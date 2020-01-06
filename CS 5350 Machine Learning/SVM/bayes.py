import numpy as np
import libsvm
from data import Data
import csv


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

        prediction = 1 if prob_x_poz.sum() > prob_x_neg.sum() else -1

        return prediction != y

    def _compute_prob_x_given_y(self, x, count_x_y, count_y):
        S_i = 2  # the number of all possible values that xi can take in the data.

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
        for test_example in test_data:
            prediction = self.predict(test_example)  # true is wrong, false is correct
            y_true = test_example[0]

            if prediction:
                wrong_predictions += 1

        accuracy = 1.0 - (wrong_predictions / len(test_data))
        return accuracy


def setup_data(data_set):
    if data_set == "s":
        libsvm_to_csv("data_semeion/hand_data_train")
        libsvm_to_csv("data_semeion/hand_data_test")
        train_data = np.loadtxt("data_semeion/hand_data_traincsv", delimiter=',', dtype=str)
        test_data = np.loadtxt("data_semeion/hand_data_testcsv", delimiter=',', dtype=str)
        train = Data(data=train_data)
        test = Data(data=test_data)
        attributes = train.attributes
        labels = []
        for i in range(1, len(train.raw_data)):
            labels.append(train.raw_data[i][0])
        return train, test, attributes, labels
        for i in range(1,5):
            path = "data_semeion/folds/fold"+str(i)
            libsvm_to_csv(path)

    elif data_set == "m":
        libsvm_to_csv("data_madelon/madelon_data_train")
        libsvm_to_csv("data_madelon/madelon_data_test")
        train_data = np.loadtxt("data_madelon/madelon_data_traincsv", delimiter=',', dtype=str)
        test_data = np.loadtxt("data_madelon/madelon_data_testcsv", delimiter=',', dtype=str)
        train = Data(data=train_data)
        test = Data(data=test_data)
        attributes = train.attributes
        labels = []
        for i in range(1, len(train.raw_data)):
            labels.append(train.raw_data[i][0])
        return train, test, attributes, labels
        for i in range(1,5):
            path = "data_madelon/folds/fold"+str(i)
            libsvm_to_csv(path)


def cvf(data_set, lr):
    acc = []
    path = ""
    if data_set == "s":
        path = "data_semeion/folds"
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
                nb = NaiveBayes(lr)
                nb.train(testing)
                rate+=nb.report(testing)
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
    #TODO train and test
    train_data = [list(map(int, i)) for i in training.raw_data]
    nb_train = NaiveBayes(hp)
    nb_train.train(train_data)
    train_acc = nb_train.report(train_data)
    avg = sum(folds)/len(folds)
    
    test_data = [list(map(int, i)) for i in testing.raw_data]
    nb_train = NaiveBayes(hp)
    nb_train.train(test_data)
    test_acc = nb_train.report(test_data)
    print("Accuracy on cross-validation for semeion data: ", avg)
    print("Accuracy on training data for semeion data: ", train_acc)
    print("Accuracy on test data for semeion data: ", test_acc)

    # train_m, test_m, attr_m, labels_m = setup_data("m")
    # folds_m = []
    # folds_m.append(cvf("m", 10))
    # folds_m.append(cvf("m", 50))
    # folds_m.append(cvf("m", 100))
    # best_m = np.max(folds_m)
    # hpm = best_hp(folds_m, best_m)
    # #TODO train and accuracy
    # train_acc_m = train(train_m, attr_m, labels_m, hpm)
    # test_acc_m = train(test_m, attr_m, labels_m, hpm)
    # avgm = sum(folds_m) / len(folds_m)
    # print("Accuracy on cross-validation for madelon data: ", avgm)
    # print("Accuracy on training data for madelon data: ", train_acc_m)
    # print("Accuracy on test data for madelon data: ", test_acc_m)