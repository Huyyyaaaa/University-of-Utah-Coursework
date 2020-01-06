from data import Data
import numpy as np
from math import log, sqrt
from collections import Counter


class Learner(object):
    pass

class Tree(object):
    def __init__(self, value):
        self.value = value
        self.children = {}

    def add_child(self, tree):
        self.children.append(tree)

def entropy(labels):
    size = float(len(labels))
    counts = Counter(labels)
    return sum(-counts[k]/size*log(counts[k]/size, 2) for k in counts)

def majority_error(labels):
    maj = float(labels.count(max(labels)))
    return 1 - maj/len(labels)

class DecisionTree(Learner):
    def __init__(self):
        pass

    def calc_info_gain(self, current_error, features, labels):
        for choice in set(features):
            reduced_labels = [label for i, label in enumerate(labels) if features[i] == choice]
            current_error -= float(len(reduced_labels))/len(labels)*self.calc_error(reduced_labels)
            return current_error

    def get_common_feature(self, observations, labels, feature_indices):
        current_error = self.calc_error(labels)
        best_feat_global = best_feat_local = bestIG = -1
        for feature in range(len(feature_indices)):
            feat_observations = [observation[feature] for observation in observations]
            IG = self.calc_info_gain(current_error, feat_observations, labels)
            if IG > bestIG:
                bestIG = IG
                best_feat_local = feature
                best_feat_global = feature_indices[feature]

        return best_feat_global, best_feat_local

    def build_tree(self, observations, labels, feature_indices, depth):
        if depth > self.max_depth:
            self.max_depth = depth

        counts = Counter(labels)
        if len(counts) == 1  or depth >= self.maxDepth:
            return Tree(max(counts, key=lambda k: counts[k]))

        bfg, bfl = self.get_common_feature(observations, labels, feature_indices)
        tree = Tree(bfg)
        reducedFeatureIndices = [el for el in feature_indices if el != bfg]
        
        for choice in set([observation[bfl] for observation in observations]):
            reducedObservations = [[el for i,el in enumerate(observation) if i != bfl] for observation in observations if observation[bfl] == choice]
            reducedLabels = [label for i,label in enumerate(labels) if observations[i][bfl] == choice]
            tree.children[choice] = self.build_tree(reducedObservations, reducedLabels, reducedFeatureIndices, depth + 1)

        return tree
    
    def train(self, observations, labels, maxDepth=-1, error=entropy):
        self.maxDepth = maxDepth if maxDepth > 0 else float('inf')
        self.max_depth = 1
        self.calc_error = error
        self.tree = self.build_tree(observations, labels, range(len(observations[0])), 1)
        return self.max_depth

    def test(self, observations, _labels):
        results = []
        for i,observation in enumerate(observations):
            results.append((self.predict(observation), _labels[i]))

        return results

    def predict(self, observation):
            tree = self.tree
        while tree.children:
            feature = observation[tree.value]
            if feature in tree.children:
                tree = tree.children[feature]
            else: 
                tree = tree.children[list(tree.children)[0]]

        return tree.value


def run(maxDepth=-1):
    obersvations = train_data

    classifier = DecisionTree()
    depth = classifier.train(obersvations, labels, maxDepth=maxDepth)

    print("Achieved Depth: ", depth)
    results = classifier.test(obersvations, labels)
    countCorrect = len([i for i in results if i[0] == i[1]])
    print ('Train Accuracy: ', countCorrect,'/', len(results),'=', float(countCorrect)/len(results))

    observations = test_data
    test_labels = []
    for i in range(len(test_data)):
        test_labels.append(test_data[i][0])

    results = classifier.test(observations, test_labels)
    countCorrect = len([i for i in results if i[0] == i[1]])
    print("Test Accuracy: ", countCorrect, '/', len(results), '=', float(countCorrect/len(results)))


def setup_data():
 
    global attributes, labels, train_data, test_data
    
    training_data = np.loadtxt("./data/train.csv", delimiter=',', dtype=str)
    t_data = Data(data=training_data)
    testing_data = np.loadtxt("./data/test.csv", delimiter=',', dtype=str)
    ts_data = Data(data=testing_data)
    attributes = t_data.attributes
    train_data = t_data.raw_data
    test_data = ts_data.raw_data
    labels =[]
    for i in range(len(t_data.raw_data)):
        labels.append(t_data.raw_data[i][0])

if __name__ == '__main__':
 
    setup_data()
    run()
