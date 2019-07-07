#include "simonmodel.h"
#include <stdlib.h>
#include <QDebug>
#include <iostream>

void Simon::addRandomButtonToSequence()
{
     char randomChar = 'A' + rand() % 2; //Generate an 'A' or a 'B' randomly
     std::cout << "add: "<< randomChar << std::endl;
    sequence.push_back(randomChar);
    if(flashDelay > 100) //lowest delay is 100
        flashDelay -= 50;
}

Simon::Simon()
{
    initialize();
}

void Simon::initialize()
{
    resetSequenceIndex();
    flashDelay = 500;
    currentSequenceIndex = 0;
    sequence.clear();

    for(int i = 0; i < 4; i++) //create
    {
        char randomChar = 'A' + rand() % 2; //Generate an 'A' or a 'B' randomly
        std::cout << randomChar << std::endl;

        sequence.push_back(randomChar);
    }
}

unsigned long Simon::getSequenceSize()
{
    return sequence.size();
}

char Simon::getNextButtonInSequence()
{
    char c = sequence[currentSequenceIndex++];
    return c;
}

int Simon::getFlashDelay()
{
    return flashDelay;
}

void Simon::resetSequenceIndex()
{
    currentSequenceIndex = 0;
}

int Simon::turnProgressPercentage()
{
    return (100 * currentSequenceIndex) / sequence.size();
}
