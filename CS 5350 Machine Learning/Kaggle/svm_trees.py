import copy
import random
import numpy as np
import libsvm
import csv
from data import Data
import dt as dt
import svm

def add_bias(arr):
    new = np.ones((arr.shape[0], 1))
    arr = np.append(arr, new, axis=1)
    return arr


def train(data, attributes, labels, k):

    trees = []
    for i in range(k):
        new_data = data
        sample = data.raw_data[np.random.choice(data.raw_data.shape[0], 100, replace=True)]
        new_data.raw_data = sample
        numbers = np.random.randint(1, data.raw_data.shape[1]-1, size=50)
        features = copy.deepcopy(attributes)
        for attr in attributes:
            if int(attr) not in numbers:
                del features[attr]
        new_data.attributes = features
        tree = dt.id3(new_data, features, labels)
        pruned = dt.pruning_tree(tree, 1)
        trees.append(pruned)
        err, depth = dt.report_error(new_data, pruned)

    transformed_data = np.zeros((data.raw_data.shape[0], k+1))
    labels = []

    for row, test in enumerate(data.raw_data):
        transformed_data[row, 0] = test[0]
    for col, tree in enumerate(trees, 1):
        label = dt.predict(data, test, tree)
        transformed_data[row, col] = int(label)
        labels.append(int(label))

    labels.append(1)
    lbls = transformed_data[:,0]
    w, a, lab = svm.train(transformed_data, lbls, k)
    return a, lab


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

        t,r = train(traind, attributes, labels, lr)
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

                r, l = train(testd, attributes_test, t_labels, lr)
                rate += r
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
    folds.append(cvf("s", 10))
    folds.append(cvf("s", 50))
    folds.append(cvf("s",100))

    best = np.max(folds)
    hp = best_hp(folds, best)
    train_acc, tl = train(training, attributes, labels, hp)
    test_acc, tel = train(testing, attributes, labels, hp)
    avg = sum(folds)/len(folds)
    print("Accuracy on cross-validation for semeion data: ", avg)
    print("Accuracy on training data for semeion data: ", train_acc)
    print("Accuracy on test data for semeion data: ", test_acc)

    ids = np.fromfile('../Kaggle/data/data-splits/eval.id', dtype=int, sep="\n")
    with open('svm_trees.csv', mode='w+', newline='') as csv_file:
        fieldnames = ['example_id', 'label']
        writer = csv.DictWriter(csv_file, fieldnames=fieldnames)
        writer.writeheader()
        for i in range(len(ids)):
            writer.writerow({'example_id': ids[i], 'label': tel[i]})
