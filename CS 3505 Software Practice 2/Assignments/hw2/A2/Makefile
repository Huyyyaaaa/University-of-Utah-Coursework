# Aaron Templeton
# U0734119
# CS3505
#A2: Classes and Facades

CC = g++
FLAGS = -Wall

LIBHARU = ../libharu
MYLIB = ./lib/
MYSRC = ./src/
END = -lhpdfs

spiralPDF: $(MYSRC)spiralPDF.cpp Spiral.o HaruPDF.o
	$(CC) $(FLAGS) -o spiralPDF Spiral.o HaruPDF.o $(MYSRC)spiralPDF.cpp -L$(LIBHARU)/src -I$(MYLIB) -I$(LIBHARU)/include $(END)


HaruPDF.o: $(MYSRC)HaruPDF.cpp $(MYLIB)HaruPDF.h
		$(CC) $(FLAGS) -c $(MYSRC)HaruPDF.cpp -L$(LIBHARU)/src -I$(MYLIB) -I$(LIBHARU)/include $(END)

Spiral.o: $(MYSRC)Spiral.cpp $(MYLIB)Spiral.h
	$(CC) $(FLAGS) -c  $(MYSRC)Spiral.cpp -I$(MYLIB)

clean:
	rm spiralPDF *.o *.pdf

test:
	./spiralPDF "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog."
