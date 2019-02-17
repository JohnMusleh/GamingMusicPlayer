using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using NAudio.CoreAudioApi;

namespace GamingMusicPlayer.MusicPlayer
{
    class VolumeMixer
    {
        public class Application
        {
            public string name;
            public int pid;
            public float peak;
            public Application(string n, int p,float pk) { this.name = n; this.pid = p; this.peak = pk; }
        }
        public class PeakChangedArgs : EventArgs
        {
            public PeakChangedArgs(Application _app) { this.app = _app; }
            public Application app;
        }
        public delegate void PeakChangedHandler(object sender, PeakChangedArgs e);

        public event PeakChangedHandler OnPeakChanged;

        public bool Running { get { return running; } }

        private Thread listener;
        private bool running;
        private List<Application> activeApps;
        private Object activeAppsLock = new Object();

        private List<Application> subscribedApps;
        private object subscribedAppsLock = new Object();

        

        public VolumeMixer()
        {
            activeApps = new List<Application>();
            subscribedApps = new List<Application>();
            listener = null;
            running = false;
            OnPeakChanged = tick;
        }

        public void startListening()
        {
            if (!running)
            {
                listener = new Thread(new ThreadStart(run));
                listener.Start();
            }
        }

        public void stopListening()
        {
            running = false;
        }

        private void tick(object sender, PeakChangedArgs e)
        {
            //Console.WriteLine(e.app.name+" -volume peak:"+e.app.peak);
        }

        private void run()
        {
            running = true;
            while (running)
            {
                updateActiveApps();
                foreach(Application a in activeApps)
                {
                    lock (subscribedAppsLock)
                    {
                        for (int i = 0; i < subscribedApps.Count; i++)
                        {
                            if (subscribedApps[i].name.Equals(a.name))
                            {
                                if (subscribedApps[i].peak != a.peak)
                                {
                                    subscribedApps[i].peak = a.peak;
                                    OnPeakChanged(this, new PeakChangedArgs(a));
                                }
                            }
                        }
                    }
                    //Console.Write(a.name+":"+a.peak+"     ");
                    
                }
               //Console.WriteLine("|");
            }

        }

        public void updateActiveApps()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
            var sessions = device.AudioSessionManager.Sessions;
            lock (activeAppsLock)
            {
                activeApps.Clear();
                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    try
                    {
                        var process = Process.GetProcessById((int)session.GetProcessID);
                        activeApps.Add( new Application(process.ProcessName, (int)session.GetProcessID, session.AudioMeterInformation.MasterPeakValue) );
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
            
        }

        public void subscribeApp(string name)
        {
            lock (subscribedAppsLock)
            {
                subscribedApps.Add(new Application(name,-1,0));
            }
        }

        public void unsubscribeApp(string name)
        {
            lock (subscribedAppsLock)
            {
                for(int i=0; i<subscribedApps.Count; i++)
                {
                    if (subscribedApps[i].name.Equals(name))
                    {
                        subscribedApps.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

}
