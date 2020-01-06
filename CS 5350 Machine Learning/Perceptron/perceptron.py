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

num_updates = 0 
avg_w = None
avg_b = 0
pocket_w = None
pocket_b = 0
pocket_a = 0

# Color settings for plotting
colors = ['darkred', 'royalblue']
colors_region = ['mistyrose', 'lightsteelblue']
cmap = ListedColormap(colors)
cmap_region = ListedColormap(colors_region)

np.random.seed(0) #Setting a random seed is important for reproducibility

def generate_data(num_samples):
    size = num_samples // 2
    x1 = np.random.multivariate_normal([0, 0], np.eye(2), size)
    y1 = -np.ones(size).astype(int)
    x2 = np.random.multivariate_normal([3, 3], np.eye(2), size)
    y2 = np.ones(size).astype(int)
    
    X = np.vstack((x1, x2))
    y = np.append(y1, y2)
    
    return X, y

def plot(x, y):
    fig = plt.figure(figsize = (7, 5), dpi = 100, facecolor = 'w')
    plt.scatter(x[:, 0], x[:, 1], c=y, edgecolor='black', cmap=cmap)
    plt.show()

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
            res = np.append(res, 1 if np.dot(new_X, w) >= 0 else -1)
            return res[0]
        else:
            new_item = np.append(item, [1])
            res = np.append(res, 1 if np.dot(new_item, w) >= 0 else -1)
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
            res.append(-1)
    
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
    if averaged:
        global avg_w, avg_b
        avg_w = np.random.uniform(0,0,size=X_train.shape[1])
        avg_b = 0
    if pocket:
        global pocket_w
        pocket_w = np.random.uniform(0,0,size=X_train.shape[1])
    b = 0 #initialize bias
    
    #FILL IN
    accuracies=[]
    for e in range(epochs):
        shuffle_arrays(X_train, y_train)
        for i in range(X_train.shape[0]):
            if decaying:
                lr = lr/(1+i)
            res = predict(X_train[i], w, b)
            if res != y_train[i]:
                w,b = update(X_train[i], y_train[i], w, b, lr)
            if averaged:
                avg_w+=w
                avg_b = (avg_b+b)
        a = accuracy(X_train, y_train, w, b)
        global pocket_a, pocket_b
        if pocket and a > pocket_a:
            pocket_a = a
            pocket_w = w
            pocket_b = b
        accuracies.append(a)
    
    if averaged:
        return avg_w, avg_b, hist, accuracies
    if pocket:
        return pocket_w, pocket_b, hist, accuracies
    return w, b, hist, accuracies

'''
    Given all the updates during training, creates an animation that shows how the decision boundary was affected.
'''
def visualize(X, Y, epoch_hist):
    fig = plt.figure(figsize = (7, 5), dpi = 100, facecolor = 'w')
    x_min, x_max = X[:, 0].min() - 1, X[:, 0].max() + 1
    y_min, y_max = X[:, 1].min() - 1, X[:, 1].max() + 1
    xx, yy = np.meshgrid(np.arange(x_min, x_max, 0.02),
                         np.arange(y_min, y_max, 0.02))
    plt.scatter(X[:, 0], X[:, 1], c=Y, edgecolor='black', cmap=cmap)
    plots = []
    for e in epoch_hist:
        epoch_values = epoch_hist[e]
        w_hist = epoch_values['w_hist']
        b_hist = epoch_values['b_hist']
        acc_hist = epoch_values['acc_hist']
        point_hist = epoch_values['point_hist']
        for i in range(len(w_hist)):
            w, b = w_hist[i], b_hist[i]     
            acc = acc_hist[i]
            if i+1 < len(point_hist):
                p_x, p_y = point_hist[i+1]
            else:
                p_x, p_y = point_hist[i]
            Z = predict(np.c_[xx.ravel(), yy.ravel()], w, b)
            Z = Z.reshape(xx.shape)
            plot =  plt.contourf(xx, yy, Z, cmap=cmap_region)
            text = f'Epoch: {e + 1} - Accuracy: {round(acc, 3)}'
            te = plt.text(90, 90, text)
            an = plt.annotate(text, xy=(0.3, 1.05), xycoords='axes fraction')
            points = plt.scatter(X[:, 0], X[:, 1], c=Y, edgecolor='black', cmap=cmap)
            c_idx = 1 if p_y == 1 else 0
            if i+1 < len(point_hist):
                p = plt.scatter(x=p_x[0], y=p_x[1], s=100, c=colors[c_idx], edgecolor='black')
                plots.append(plot.collections + [te, an, points, p])
            else:
                plots.append(plot.collections + [te, an, points])
    return animation.ArtistAnimation(fig, plots, repeat=False, blit=False, interval=500)


def setup():
    X_train, y_train, num_features = libsvm.read_libsvm('data_train')
    X_test, y_test, _ = libsvm.read_libsvm('data_test', num_features)
    return X_train, y_train, num_features, X_test, y_test

def CVF(hp, decaying=False, averaged=False):
    folds=[]
    for i in range(5):
        file = "./CVfolds/fold"+str(i+1)
        cv_x_train, cv_y_train, cv_num_features = libsvm.read_libsvm(file)
        if decaying:
            cv_w, cv_b, cv_hist, accs = train(cv_x_train.toarray(), cv_y_train, epochs=10, lr=hp, decaying=True)
        elif averaged:
            cv_w, cv_b, cv_hist, accs = train(cv_x_train.toarray(), cv_y_train, epochs=10, lr=hp, averaged=True) 
        else:
            cv_w, cv_b, cv_hist, accs = train(cv_x_train.toarray(), cv_y_train, epochs=10, lr=hp)
        temp = 0
        for j in range(5):
            if i != j:
                f = "./CVfolds/fold"+str(j+1)
                x, y,_ = libsvm.read_libsvm(f, cv_num_features)
                temp += accuracy(x.toarray(), y, cv_w, cv_b)
        folds.append(temp/4)
    return sum(folds)/len(folds)
    pass 

#get top 10 and bottom 10 words
def maj(w):
    f = open("vocab_idx.json")
    s = f.read()   
    v = json.loads(s)
    top = np.argsort(w)[:10]
    bottom = np.argsort(w)[-10:]
    print("Top 10 words")
    for w in top:
        print(v[str(w)])
    print("Bottom 10 words")
    for w in bottom:
        print(v[str(w)])

if __name__ == '__main__':
    random.seed(0)
    X_train, y_train, num_features, x_test, y_test = setup()
    
    fold_rates=[]
    #run CV for each hyper-parameter
    fold_rates.append(CVF(1))
    fold_rates.append(CVF(.1))
    fold_rates.append(CVF(.01))
    best_rate = np.amax(fold_rates)
    #run train data for 20 epochs and get accuracy for each
    w,b,hist, accs = train(X_train.toarray(), y_train, epochs=20, lr=best_rate)
    test_acc = accuracy(x_test.toarray(), y_test, w, b)
    
    bhp =0
    if(fold_rates.index(best_rate)==0):
         bhp = 1
    elif(fold_rates.index(best_rate)==1):
        bhp = .1
    else:
        bhp = .01
    
    print("--SIMPLE PERCEPTRON--")
    print("best hyper-paramter: ", bhp)
    print("cross-validation accuracy for hyper-parameter: ", best_rate)
    print("number of updates: ", num_updates)
    print("training accuracy: ", np.max(accs))
    print("test accuracy: ", test_acc)
    plt.plot(accs)
    plt.ylabel("Training Accuracy")
    plt.title("Simple Perceptron")
    plt.show()
    #Perceptron with decaying learning rate

    fold_rates=[]
    num_updates = 0
    fold_rates.append(CVF(1, decaying=True))
    fold_rates.append(CVF(.1, decaying=True))
    fold_rates.append(CVF(.01, decaying=True))
    best_rate = np.amax(fold_rates)
    w,b,hist, accs = train(X_train.toarray(), y_train, epochs=20, lr=best_rate, decaying=True)
    test_acc = accuracy(x_test.toarray(), y_test, w, b)
    
    bhp =0
    if(fold_rates.index(best_rate)==0):
         bhp = 1
    elif(fold_rates.index(best_rate)==1):
        bhp = .1
    else:
        bhp = .01
    
    print("--DECAYING RATE PERCEPTRON--")
    print("best hyper-paramter: ", bhp)
    print("cross-validation accuracy for hyper-parameter: ", best_rate)
    print("number of updates: ", num_updates)
    print("training accuracy: ", np.max(accs))
    print("test accuracy: ", test_acc)
    plt.plot(accs)
    plt.ylabel("Training Accuracy")
    plt.title("Decaying Rate Perceptron")
    plt.show()
    #averaged perceptron

    fold_rates=[]
    #global num_updates
    num_updates = 0
    fold_rates.append(CVF(1, averaged=True))
    fold_rates.append(CVF(.1, averaged=True))
    fold_rates.append(CVF(.01, averaged=True))
    best_rate = np.amax(fold_rates)
    w,b,hist, accs = train(X_train.toarray(), y_train, epochs=20, lr=best_rate, averaged=True)
    test_acc = accuracy(x_test.toarray(), y_test, w, b)
    
    bhp =0
    if(fold_rates.index(best_rate)==0):
         bhp = 1
    elif(fold_rates.index(best_rate)==1):
        bhp = .1
    else:
        bhp = .01
    
    print("--AVERAGED PERCEPTRON--")
    print("best hyper-paramter: ", bhp)
    print("cross-validation accuracy for hyper-parameter: ", best_rate)
    print("number of updates: ", num_updates)
    print("training accuracy: ", np.max(accs))
    print("test accuracy: ", test_acc)
    plt.plot(accs)
    plt.ylabel("Training Accuracy")
    plt.title("Averaged Perceptron")
    plt.show()
    maj(w)
    #Pocket Perceptron

    fold_rates=[]
    #global num_updates
    num_updates = 0
    fold_rates.append(CVF(1))
    fold_rates.append(CVF(.1))
    fold_rates.append(CVF(.01))
    best_rate = np.amax(fold_rates)
    w,b,hist, accs = train(X_train.toarray(), y_train, epochs=20, lr=best_rate, pocket=True)
    test_acc = accuracy(x_test.toarray(), y_test, w, b)
    
    bhp =0
    if(fold_rates.index(best_rate)==0):
         bhp = 1
    elif(fold_rates.index(best_rate)==1):
        bhp = .1
    else:
        bhp = .01
    
    print("--POCKET PERCEPTRON--")
    print("best hyper-paramter: ", bhp)
    print("cross-validation accuracy for hyper-parameter: ", best_rate)
    print("number of updates: ", num_updates)
    print("training accuracy: ", np.max(accs))
    print("test accuracy: ", test_acc)
    plt.plot(accs)
    plt.ylabel("Training Accuracy")
    plt.title("Pocket Perceptron")
    plt.show()
    
    '''
    X_train, y_train = generate_data(150)
    plot(X_train, y_train)
    # setup data for testing
    w_t = np.array([1., -1.])
    b_t = 0.5
    X_t = np.array([-0.1, 0.8, 0.5, 0.5, -0.5, -0.5, 0, 0, -0.5, 0.5]).reshape(5, 2)
    x_t = np.array([0.3, 0.3])
    y_t = -1
    lr_t = 0.1

  

    predict(X_t, w_t, b_t) 
    predict(x_t, w_t, b_t)  #output should be: 1.0
    accuracy(X_t, np.ones(X_t.shape[0]), w_t, b_t) #output should be 0.6
    update(x_t, y_t, w_t, b_t, lr_t) #output should be: (array([ 0.97, -1.03]), 0.4)

    w, b, hist = train(X_train, y_train, epochs=10, lr=0.01)
    accuracy(X_train, y_train, w, b)

    ani = visualize(X_train, y_train, hist.training_hist)
    HTML(ani.to_html5_video()) 
    # ani.save('perceptron.mp4') #If you want to store the animation as a video
    '''