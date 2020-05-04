# multiAgents.py
# --------------
# Licensing Information:  You are free to use or extend these projects for
# educational purposes provided that (1) you do not distribute or publish
# solutions, (2) you retain this notice, and (3) you provide clear
# attribution to UC Berkeley, including a link to http://ai.berkeley.edu.
# 
# Attribution Information: The Pacman AI projects were developed at UC Berkeley.
# The core projects and autograders were primarily created by John DeNero
# (denero@cs.berkeley.edu) and Dan Klein (klein@cs.berkeley.edu).
# Student side autograding was added by Brad Miller, Nick Hay, and
# Pieter Abbeel (pabbeel@cs.berkeley.edu).
import math

from util import manhattanDistance
from game import Directions
import random, util

from game import Agent

class ReflexAgent(Agent):
    """
    A reflex agent chooses an action at each choice point by examining
    its alternatives via a state evaluation function.

    The code below is provided as a guide.  You are welcome to change
    it in any way you see fit, so long as you don't touch our method
    headers.
    """


    def getAction(self, gameState):
        """
        You do not need to change this method, but you're welcome to.

        getAction chooses among the best options according to the evaluation function.

        Just like in the previous project, getAction takes a GameState and returns
        some Directions.X for some X in the set {NORTH, SOUTH, WEST, EAST, STOP}
        """
        # Collect legal moves and successor states
        legalMoves = gameState.getLegalActions()

        # Choose one of the best actions
        scores = [self.evaluationFunction(gameState, action) for action in legalMoves]
        bestScore = max(scores)
        bestIndices = [index for index in range(len(scores)) if scores[index] == bestScore]
        chosenIndex = random.choice(bestIndices) # Pick randomly among the best

        "Add more of your code here if you want to"

        return legalMoves[chosenIndex]

    def evaluationFunction(self, currentGameState, action):
        """
        Design a better evaluation function here.

        The evaluation function takes in the current and proposed successor
        GameStates (pacman.py) and returns a number, where higher numbers are better.

        The code below extracts some useful information from the state, like the
        remaining food (newFood) and Pacman position after moving (newPos).
        newScaredTimes holds the number of moves that each ghost will remain
        scared because of Pacman having eaten a power pellet.

        Print out these variables to see what you're getting, then combine them
        to create a masterful evaluation function.
        """
        # Useful information you can extract from a GameState (pacman.py)
        successorGameState = currentGameState.generatePacmanSuccessor(action)
        newPos = successorGameState.getPacmanPosition()
        newFood = successorGameState.getFood()
        newGhostStates = successorGameState.getGhostStates()
        newScaredTimes = [ghostState.scaredTimer for ghostState in newGhostStates]
        # print(newPos, newFood, newScaredTimes)
        "*** YOUR CODE HERE ***"
        # return successorGameState.getScore()
        distGhost = [(g.getPosition()[0], g.getPosition()[1]) for g in newGhostStates]
        ghostScared = min(newScaredTimes) > 0

        if not ghostScared and (newPos in distGhost):
            return -1
        if newPos in currentGameState.getFood().asList():
            return 1

        distKey = lambda x: util.manhattanDistance(x, newPos)
        minFood = sorted(newFood.asList(), key=distKey)
        minGhost = sorted(distGhost, key=distKey)

        return 1.0 / distKey(minFood[0]) - 1.0 / distKey(minGhost[0])


def scoreEvaluationFunction(currentGameState):
    """
    This default evaluation function just returns the score of the state.
    The score is the same one displayed in the Pacman GUI.

    This evaluation function is meant for use with adversarial search agents
    (not reflex agents).
    """
    return currentGameState.getScore()

class MultiAgentSearchAgent(Agent):
    """
    This class provides some common elements to all of your
    multi-agent searchers.  Any methods defined here will be available
    to the MinimaxPacmanAgent, AlphaBetaPacmanAgent & ExpectimaxPacmanAgent.

    You *do not* need to make any changes here, but you can if you want to
    add functionality to all your adversarial search agents.  Please do not
    remove anything, however.

    Note: this is an abstract class: one that should not be instantiated.  It's
    only partially specified, and designed to be extended.  Agent (game.py)
    is another abstract class.
    """

    def __init__(self, evalFn = 'scoreEvaluationFunction', depth = '2'):
        self.index = 0 # Pacman is always agent index 0
        self.evaluationFunction = util.lookup(evalFn, globals())
        self.depth = int(depth)

class MinimaxAgent(MultiAgentSearchAgent):
    """
    Your minimax agent (question 2)
    """

    def getAction(self, gameState):
        """
        Returns the minimax action from the current gameState using self.depth
        and self.evaluationFunction.

        Here are some method calls that might be useful when implementing minimax.

        gameState.getLegalActions(agentIndex):
        Returns a list of legal actions for an agent
        agentIndex=0 means Pacman, ghosts are >= 1

        gameState.generateSuccessor(agentIndex, action):
        Returns the successor game state after an agent takes an action

        gameState.getNumAgents():
        Returns the total number of agents in the game

        gameState.isWin():
        Returns whether or not the game state is a winning state

        gameState.isLose():
        Returns whether or not the game state is a losing state
        """
        "*** YOUR CODE HERE ***"
        agents = [g for g in range(1, gameState.getNumAgents())]

        def val(state, depth):
            return state.isLose() or state.isWin() or depth == self.depth

        def minVal(state, depth, agent):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = float("inf")
            for action in state.getLegalActions(agent):
                if agent == agents[-1]:
                    res = min(res, maxVal(state.generateSuccessor(agent, action), depth + 1))
                else:
                    res = min(res, minVal(state.generateSuccessor(agent, action), depth, agent + 1))
            return res

        def maxVal(state, depth):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = float("-inf")
            for action in state.getLegalActions(0):
                res = max(res, minVal(state.generateSuccessor(0, action), depth, 1))
            return res

        result = [(x, minVal(gameState.generateSuccessor(0, x), 0, 1)) for x in gameState.getLegalActions(0)]
        result.sort(key=lambda x: x[1])
        # print("result {0}".format(result[-1][0]))
        return result[-1][0]


class AlphaBetaAgent(MultiAgentSearchAgent):
    """
    Your minimax agent with alpha-beta pruning (question 3)
    """

    def getAction(self, gameState):
        """
        Returns the minimax action using self.depth and self.evaluationFunction
        """
        "*** YOUR CODE HERE ***"
        agents = [g for g in range(1, gameState.getNumAgents())]
        infinity = float("inf")

        def val(state, depth):
            return state.isLose() or state.isWin() or depth == self.depth

        def minVal(state, depth, agent, alpha, beta):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = float("inf")
            for action in state.getLegalActions(agent):
                if agent == agents[-1]:
                    res = min(res, maxVal(state.generateSuccessor(agent, action), depth + 1, alpha, beta))
                else:
                    res = min(res, minVal(state.generateSuccessor(agent, action), depth, agent + 1, alpha, beta))

                if res < alpha:
                    return res
                beta = min(beta, res)

            return res

        def maxVal(state, depth, alpha, beta):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = float("-inf")
            for action in state.getLegalActions(0):
                res = max(res, minVal(state.generateSuccessor(0, action), depth, 1, alpha, beta))
                if res > beta:
                    return res
                alpha = max(alpha, res)
            return res

        def alphaBetaPrune(state):

            res = float("-inf")
            n = None
            alpha = float("-inf")
            beta = float("inf")

            for action in state.getLegalActions(0):
                val = minVal(gameState.generateSuccessor(0, action), 0, 1, alpha, beta)
                if res < val:
                    res = val
                    n = action
                if res > beta:
                    return res
                alpha = max(alpha, res)

            return n

        return alphaBetaPrune(gameState)

class ExpectimaxAgent(MultiAgentSearchAgent):
    """
      Your expectimax agent (question 4)
    """

    def getAction(self, gameState):
        """
        Returns the expectimax action using self.depth and self.evaluationFunction

        All ghosts should be modeled as choosing uniformly at random from their
        legal moves.
        """
        "*** YOUR CODE HERE ***"
        agents = [g for g in range(1, gameState.getNumAgents())]

        def val(state, depth):
            return state.isLose() or state.isWin() or depth == self.depth

        def expectedVal(state, depth, agent):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = 0
            prob = 1 / len(state.getLegalActions(agent))

            for action in state.getLegalActions(agent):
                if agent == agents[-1]:
                    res += prob * maxVal(state.generateSuccessor(agent, action), depth+1)
                else:
                    res += prob * expectedVal(state.generateSuccessor(agent, action), depth, agent +1)
            return res

        def maxVal(state, depth):
            if val(state, depth):
                return self.evaluationFunction(state)

            res = float("-inf")
            for action in state.getLegalActions(0):
                res = max(res, expectedVal(state.generateSuccessor(0, action), depth, 1))
            return res

        result = [(x, expectedVal(gameState.generateSuccessor(0, x), 0, 1)) for x in gameState.getLegalActions(0)]
        result.sort(key=lambda x: x[1])
        # print("result {0}".format(result[-1][0]))
        return result[-1][0]


def betterEvaluationFunction(currentGameState):
    """
    Your extreme ghost-hunting, pellet-nabbing, food-gobbling, unstoppable
    evaluation function (question 5).

    DESCRIPTION: <write something here so we know what you did>
    """
    "*** YOUR CODE HERE ***"
    newPos = currentGameState.getPacmanPosition()
    newFood = currentGameState.getFood().asList()
    newGhostStates = currentGameState.getGhostStates()
    score = 0

    ghostsPosition = [(i.getPosition()[0], i.getPosition()[1]) for i in newGhostStates]

    if currentGameState.isLose():
        return float("-inf")
    if newPos in ghostsPosition:
        return float("-inf")

    distKey = lambda x: util.manhattanDistance(x, newPos)
    closestFood = sorted(newFood, key=distKey)
    closestGhost = sorted(ghostsPosition, key=distKey)
    firstGhost = closestGhost[0]

    if distKey(firstGhost) < 3:
        score -= 500
    if distKey(firstGhost) < 2:
        score -= 1000
    if distKey(firstGhost) < 1:
        return float("-inf")
    if len(currentGameState.getCapsules()) < 2:
        score += 100
    if len(closestFood) == 0 or len(closestGhost) == 0:
        score += scoreEvaluationFunction(currentGameState) + 10
    else:
        score += (scoreEvaluationFunction(currentGameState) + 10 / distKey(closestFood[0])
                  + 1/distKey(closestGhost[0]) + 1/distKey(closestGhost[-1]))

    return score

# Abbreviation
better = betterEvaluationFunction
