import pickle


def predict(bpm, zcr, spectirr):
    filename = "model.sav"
    model = pickle.load(open(filename, 'rb'))
    return model.predict([[bpm, zcr, spectirr]])[0]

