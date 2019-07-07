#ifndef SIMONMODEL_H
#define SIMONMODEL_H

#include <vector>

class Simon{

public: //Constructors
    Simon();

private:
    std::vector<char> sequence; //'A' is left button (red), 'B' is right button (blue)
    int flashDelay; //time inbetween button flashes for Simon in ms
    unsigned long currentSequenceIndex;

public:
   void initialize();
   void addRandomButtonToSequence();
   char getNextButtonInSequence();
   int getFlashDelay();
   unsigned long getSequenceSize();
   void resetSequenceIndex();
   int turnProgressPercentage();
};

#endif // SIMONMODEL_H
