import numpy as np
import libsvm
import csv

np.random.seed(0)

def shuffle_arrays(X, y):
    idx = np.arange(X.shape[0])
    np.random.shuffle(idx)
    return X[idx], y[idx]


def accuracy(x, y, w):
    rate = 0
    res = []
    for i in range(len(x)):
        temp = np.dot(w, x[i])
        if temp >= 0:
            res.append(1)
        else:
            res.append(0)
    for i in range(len(res)):
        if res[i] == y[i]:
            rate += 1
    return rate/len(y), res


def update(x, y, w, lr, reg):
    if (y * np.dot(w, x)) <= 1:
        nw = ((1-lr)*w) + lr * reg * y * x
    else:
        nw = (1-lr) * w
    return nw


def add_bias(arr):
    new = np.ones((arr.shape[0], 1))
    arr = np.append(arr, new, axis=1)
    return arr

def train(x, y, lr, epochs=10):
    w = np.random.uniform(0, 0, size=x.shape[1])  # initialize w
    x = add_bias(x)
    w = np.append(w, 1)
    new_lr = lr
    for e in range(epochs):
        xs, ys = shuffle_arrays(x, y)
        for i in range(xs.shape[0]):
            w = update(xs[i], ys[i], w, new_lr, 1)
        new_lr = lr / (1 + e)
        a, labels = accuracy(xs, ys, w)
    return w, a, labels


def setup_data(data_set):
    if data_set == "s":
        xtr, ytr, nf = libsvm.read_libsvm("../Kaggle/data/data-splits/data.train")
        xt, yt, _ = libsvm.read_libsvm("../Kaggle/data/data-splits/data.test", nf)
        return xtr, ytr, nf, xt, yt

    elif data_set == "m":
        xtr, ytr, nf = libsvm.read_libsvm("data_madelon/madelon_data_train")
        xt, yt, _ = libsvm.read_libsvm("data_madelon/madelon_data_test", nf)
        return xtr, ytr, nf, xt, yt


def cvf(data_set, lr):
    acc = []
    path = ""
    if data_set == "s":
        path = "../Kaggle/data/cvf"
    else:
        path = "data_madelon/folds"

    for i in range(1, 5):
        file = path+"/fold"+str(i)
        cvxtrain, cvytrain, cvnum = libsvm.read_libsvm(file)
        w, a, lab = train(cvxtrain.toarray(), cvytrain, lr)
        rate = 0
        for j in range(1, 5):
            if i != j:
                f = path+"/fold"+str(j+1)
                x,y,_ = libsvm.read_libsvm(f, cvnum)
                x = add_bias(x.toarray())
                ac, l = accuracy(x, y, w)
                rate+=ac
        acc.append(rate/4)
    return sum(acc)/len(acc)


def best_hp(arr, val):
    i = arr.index(val)
    if i == 0:
        return 10**1
    elif i == 1:
        return 10**0
    elif i == 2:
        return 10**-1
    elif i == 3:
        return 10**-2
    elif i == 4:
        return 10**-3
    else:
        return 10**-4


if __name__ == '__main__':
    xtrain_s, ytrain_s, numfeat_s, xtest_s, ytest_s = setup_data("s")
    folds = []
    folds.append(cvf("s", 10**1))
    folds.append(cvf("s", 10**0))
    folds.append(cvf("s",10**-1))
    folds.append(cvf("s",10**-2))
    folds.append(cvf("s",10**-3))
    folds.append(cvf("s",10**-4))
    best = np.max(folds)
    hp = best_hp(folds, best)
    w, a, testl = train(xtrain_s.toarray(), ytrain_s, hp)
    wt, at, test2 = train(xtest_s.toarray(), ytest_s, hp)
    avg = sum(folds)/len(folds)
    print("Accuracy on cross-validation for semeion data: ", avg)
    print("Accuracy on training data for semeion data: ", a)
    print("Accuracy on test data for semeion data: ", at)

    xt, yt, _ = libsvm.read_libsvm("../Kaggle/data/data-splits/data.eval.anon")
    xf = add_bias(xt.toarray())
    fa, labels = accuracy(xf, yt, wt)
    print(fa)
    labels = np.append(labels, 1)


    ids = np.fromfile('../Kaggle/data/data-splits/eval.id', dtype=int, sep="\n")
    with open('svmsolutions.csv', mode='w+', newline='') as csv_file:
        fieldnames = ['example_id', 'label']
        writer = csv.DictWriter(csv_file, fieldnames=fieldnames)
        writer.writeheader()
        for i in range(len(ids)):
            writer.writerow({'example_id': ids[i], 'label': labels[i]})
