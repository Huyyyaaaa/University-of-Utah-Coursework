import numpy as np
import libsvm
from data import Data
import csv
import sklearn.preprocessing

class NaiveBayes:

    def __init__(self, smoothing):
        self._W_poz = np.zeros(256,)
        self._W_neg = np.zeros(256,)
        self._prior_poz = 0
        self._prior_neg = 0
        self._s = smoothing

    def train(self, train_data, epochs=20):

        for train_example in train_data:
            self._update(train_example)

    def predict(self, test_data):
        y = test_data[0]
        x = test_data[1:]

        prob_x_poz = self._compute_prob_x_given_y(x, self._W_poz, self._prior_poz)
        prob_x_neg = self._compute_prob_x_given_y(x, self._W_neg, self._prior_neg)


        prediction = 1 if sum(prob_x_poz) > sum(prob_x_neg) else 0



        return prediction != y, prediction

    def _compute_prob_x_given_y(self, x, count_x_y, count_y):
        S_i = 1
        x_neg = count_x_y * x
        count_x_neg = x_neg / count_y
        nominator_neg = count_x_neg + self._s
        denominator_neg = count_y + S_i * self._s
        prob_x_neg = nominator_neg / denominator_neg

        return prob_x_neg

    def _update(self, test_data):
        y = test_data[0]
        x = test_data[1:]

        if y > 0:
            self._W_poz += x
            self._prior_poz += 1
        else:
            self._W_neg += x
            self._prior_neg += 1

    def report(self, test_data):

        wrong_predictions = 0
        pred = []
        for test_example in test_data:
            prediction, actual = self.predict(test_example)  # true is wrong, false is correct
            y_true = test_example[0]
            pred.append(actual)
            if prediction:
                wrong_predictions += 1

        accuracy = 1.0 - (wrong_predictions / len(test_data))
        return accuracy, pred


def setup_data(data_set):
    if data_set == "s":
        libsvm_to_csv("../Kaggle/data/data-splits/data.train")
        libsvm_to_csv("../Kaggle/data/data-splits/data.test")
        train_data = np.loadtxt("../Kaggle/data/data-splits/data.traincsv", delimiter=',', dtype=str)
        test_data = np.loadtxt("../Kaggle/data/data-splits/data.testcsv", delimiter=',', dtype=str)
        train = Data(data=train_data)
        test = Data(data=test_data)
        attributes = train.attributes
        labels = []
        for i in range(1, len(train.raw_data)):
            labels.append(train.raw_data[i][0])
        return train, test, attributes, labels
        for i in range(1,5):
            path = "../Kaggle/data/cvf/"+str(i)
            libsvm_to_csv(path)

    else:
        libsvm_to_csv("../Kaggle/data/data-splits/data.eval.anon")
        libsvm_to_csv("../Kaggle/data/data-splits/data.test")
        train_data = np.loadtxt("../Kaggle/data/data-splits/data.eval.anoncsv", delimiter=',', dtype=str)
        test_data = np.loadtxt("../Kaggle/data/data-splits/data.testcsv", delimiter=',', dtype=str)
        train = Data(data=train_data)
        test = Data(data=test_data)
        attributes = train.attributes
        labels = []
        for i in range(1, len(train.raw_data)):
            labels.append(train.raw_data[i][0])
        return train, test, attributes, labels
        for i in range(1, 5):
            path = "../Kaggle/data/cvf/" + str(i)
            libsvm_to_csv(path)



def cvf(data_set, lr):
    acc = []
    path = ""
    if data_set == "s":
        path = "../Kaggle/data/cvf"
        for i in range(1,6):
            p = path+"/fold"+str(i)
            libsvm_to_csv(p)
    else:
        path = "data_madelon/folds"
        for i in range(1,6):
            p = path+"/fold"+str(i)
            libsvm_to_csv(p)

    for i in range(1, 5):
        file = path+"/fold"+str(i)+"csv"
        train_data = np.loadtxt(file, delimiter=',', dtype=str)
        traind = Data(data=train_data)
        attributes = traind.attributes
        labels = []
        for i in range(1, len(traind.raw_data)):
            labels.append(traind.raw_data[i][0])
        training = [list(map(int, i)) for i in traind.raw_data]
        training = sklearn.preprocessing.binarize(training, threshold=0.0)
        nb_train = NaiveBayes(lr)
        nb_train.train(training)
        nb_train.report(training)
        rate = 0
        for j in range(1, 5):
            if i != j:
                f = path+"/fold"+str(j+1)+"csv"
                test_data = np.loadtxt(f, delimiter=',', dtype=str)
                testd = Data(data=test_data)
                attributes_test = testd.attributes
                t_labels=[]
                for i in range(1, len(testd.raw_data)):
                    t_labels.append(testd.raw_data[i][0])
                testing = [list(map(int, i)) for i in testd.raw_data]
                testing = sklearn.preprocessing.binarize(testing, threshold=0)
                nb = NaiveBayes(lr)
                nb.train(testing)
                r,p = nb.report(testing)
            rate+=r
        acc.append(rate/4)
    return sum(acc)/len(acc)


def best_hp(arr, val):
    i = arr.index(val)
    if i == 0:
        return 10
    elif i == 1:
        return 50
    elif i == 2:
        return 100



def libsvm_to_csv(file):
    reader = csv.reader(open(file), delimiter=" ")
    writer = csv.writer(open(file+"csv", 'w', newline=''))

    header = []
    header.append("label")
    for n in range(1, 257):
        header.append(n)

    writer.writerow(header)

    for line in reader:
        label = line.pop(0)
        if line[-1].strip()=='':
            line.pop(-1)

        line = map(lambda x: tuple(x.split(":")), line)
        new_line = [label]+[0]*256
        for i,v in line:
            i = int(i)
            if i <= 256:
                new_line[i] = v
        writer.writerow(new_line)


if __name__ == '__main__':
    training, testing, attributes, labels = setup_data("s")
    folds = []
    folds.append(cvf("s", 2))
    folds.append(cvf("s", 1.5))
    folds.append(cvf("s",1))
    folds.append(cvf("s", 0.5))

    best = np.max(folds)
    hp = best_hp(folds, best)
    train_data = [list(map(int, i)) for i in training.raw_data]
    train_data = sklearn.preprocessing.binarize(train_data, threshold=0)
    nb_train = NaiveBayes(hp)
    nb_train.train(train_data)
    train_acc, pr = nb_train.report(train_data)
    avg = sum(folds)/len(folds)
    
    test_data = [list(map(int, i)) for i in testing.raw_data]
    test_data = sklearn.preprocessing.binarize(test_data, threshold=0)
    nb_train.train(test_data)
    test_acc, pr = nb_train.report(test_data)
    print("Accuracy on cross-validation for semeion data: ", avg)
    print("Accuracy on training data for semeion data: ", train_acc)
    print("Accuracy on test data for semeion data: ", test_acc)

    t, testing, attributes, labels = setup_data("x")
    data = [list(map(int, i)) for i in t.raw_data]
    data = sklearn.preprocessing.binarize(data, threshold=0)
    ta, pt = nb_train.report(data)
    pt.append(1)

    ids = np.fromfile('../Kaggle/data/data-splits/eval.id', dtype=int, sep="\n")
    with open('bayes.csv', mode='w+', newline='') as csv_file:
        fieldnames = ['example_id', 'label']
        writer = csv.DictWriter(csv_file, fieldnames=fieldnames)
        writer.writeheader()
        for i in range(len(ids)):
            writer.writerow({'example_id': ids[i], 'label': pt[i]})