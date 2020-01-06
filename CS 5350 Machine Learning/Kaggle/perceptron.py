import numpy as np
import matplotlib.pyplot as plt
from matplotlib.colors import ListedColormap
import matplotlib.animation as animation
from IPython.display import HTML
from sklearn import datasets
import sklearn
from scipy.sparse import csr_matrix
import libsvm 
import random
import json
import csv

num_updates = 0 
np.random.seed(0) #Setting a random seed is important for reproducibility

#PERCEPTRON
'''
    X: input vector or set of vectors
    w: weight vector
    b: bias
    
    Output: a numpy array containing the predictions produced by the linear threshold unit defined by w and b.
'''
def predict(X, w, b):
    #FILL IN
    w = np.append(w, [b])
    res = []
    for item in X:
        if not isinstance(item, np.ndarray):
            new_X = np.append(X, [1])
            res = np.append(res, 1 if np.dot(new_X, w) >= 0 else 0)
            return res[0]
        else:
            new_item = np.append(item, [1])
            res = np.append(res, 1 if np.dot(new_item, w) >= 0 else 0)
    return res
    pass


'''
    X: set of input vectors
    y: set of labels
    w: weight vector
    b: bias
    
    Output: The accuracy achieved by the classifier defined by w and b on samples X.
'''
def accuracy(X, y, w, b):
    #FILL IN
    rate = 0
    res =[]
    for i in range(len(X)):
        temp = np.dot(w, X[i]) + b
        if temp >= 0:
             res.append(1)
        else:
            res.append(0)
    
    for i in range(len(res)):
        if res[i] == y[i]:
            rate+=1

    return rate/len(y)
    pass

'''
    x: input vector
    y: label
    w: weight vector
    b: bias
    lr: learning rate
    
    Updates the w and b according to the Perceptron update rule.
    
    Output: updated w and b
'''
def update(x, y, w, b, lr):
    global num_updates
    num_updates += 1
    #FILL IN
    b = b + (lr*y)
    for i in range(len(w)):
        w[i] = w[i] + lr*(y*x[i])
    
    return w, b
    pass


'''
    A class that will handle the storage of the historic values of training
'''
class History:
    def __init__(self, num_epochs):
        self.training_hist = dict()
        for n in range(num_epochs):
            self.training_hist[n] = {'w_hist': [], 
                                'b_hist': [], 
                                'acc_hist': [],
                                'point_hist':[]}
    def store(self, x, y, w, b, accuracy, epoch):
        self.training_hist[epoch]['point_hist'].append((x, y))
        self.training_hist[epoch]['w_hist'].append(w.copy())
        self.training_hist[epoch]['b_hist'].append(b)
        self.training_hist[epoch]['acc_hist'].append(accuracy)

def shuffle_arrays(X, y):
    idx = np.arange(X.shape[0])
    np.random.shuffle(idx)
    return X[idx], y[idx]

def train(X_train, y_train, epochs=10, lr=0.01, decaying=False, averaged=False, pocket=False):
    hist = History(epochs)
    w = np.random.uniform(0, 1, size=X_train.shape[1]) #initialize w
    b = 0 #initialize bias

    accuracies=[]
    for e in range(epochs):
        shuffle_arrays(X_train, y_train)
        for i in range(X_train.shape[0]):
            res = predict(X_train[i], w, b)
            if res != y_train[i]:
                w,b = update(X_train[i], y_train[i], w, b, lr)
        a = accuracy(X_train, y_train, w, b)
        accuracies.append(a)
    return w, b, hist, accuracies

def setup():
    X_train, y_train, num_features = libsvm.read_libsvm('./data/data-splits/data.train')
    X_test, y_test, _ = libsvm.read_libsvm('./data/data-splits/data.test', num_features)
    return X_train, y_train, num_features, X_test, y_test

def CVF(hp):
    folds=[]
    for i in range(5):
        file = "./data/cvf/fold"+str(i+1)
        cv_x_train, cv_y_train, cv_num_features = libsvm.read_libsvm(file)
        cv_w, cv_b, cv_hist, accs = train(cv_x_train.toarray(), cv_y_train, epochs=10, lr=hp)
        temp = 0
        for j in range(5):
            if i != j:
                f = "./data/cvf/fold"+str(j+1)
                x, y,_ = libsvm.read_libsvm(f, cv_num_features)
                temp += accuracy(x.toarray(), y, cv_w, cv_b)
        folds.append(temp/4)
    return sum(folds)/len(folds)
    pass 

if __name__ == '__main__':
    random.seed(0)
    X_train, y_train, num_features, x_test, y_test = setup()
    
    fold_rates=[]
    #run CV for each hyper-parameter
   
    fold_rates.append(CVF(1))
    fold_rates.append(CVF(.1))
    fold_rates.append(CVF(.01))
    fold_rates.append(CVF(5))
    best_rate = np.amax(fold_rates)
    #run train data for 20 epochs and get accuracy for each
    w,b,hist, accs = train(X_train.toarray(), y_train, epochs=30, lr=best_rate)
    test_acc = accuracy(x_test.toarray(), y_test, w, b)
    y_test = np.append(y_test, y_test[0])

    ids = np.fromfile('./data/data-splits/eval.id', dtype=int, sep="\n")
    with open('p.csv', mode='w+', newline='') as csv_file:
        fieldnames = ['example_id', 'label']
        writer=csv.DictWriter(csv_file, fieldnames=fieldnames)
        writer.writeheader()
        for i in range(len(ids)):
            writer.writerow({'example_id':ids[i], 'label':y_test[i]})

    # bhp =0
    # if(fold_rates.index(best_rate)==0):
    #      bhp = 1
    # elif(fold_rates.index(best_rate)==1):
    #     bhp = .1
    # elif(fold_rates.index(best_rate)==3):
    #     bhp = .01
    # else:
    #     bhp = 5
    
    # print("--SIMPLE PERCEPTRON--")
    # print("best hyper-paramter: ", bhp)
    # print("cross-validation accuracy for hyper-parameter: ", best_rate)
    # print("number of updates: ", num_updates)
    # print("training accuracy: ", np.max(accs))
    # print("test accuracy: ", test_acc)
    # plt.plot(accs)
    # plt.ylabel("Training Accuracy")
    # plt.title("Simple Perceptron")
    # plt.show()

    
 