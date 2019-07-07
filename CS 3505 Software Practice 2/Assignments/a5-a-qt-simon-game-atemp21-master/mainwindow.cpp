#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QThread>
#include <QTimer>
#include <QTime>
#include <QDebug>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    qApp->setStyleSheet("QProgressBar::chunk{"
                        "background-color:qlineargradient(x1: 0.5, y1: 0.5, x2: 1, y2: 0.5,stop: 0 #ffff80, stop: 1 #cccc00);"
                        " border-radius:5px;"
                        "}");

}

MainWindow::~MainWindow()
{
    delete ui;
}

/////////////////////
///     SLOTS     ///
/////////////////////
void MainWindow::on_startButton_clicked()
{
    ui->startButton->setEnabled(false);
    ui->startButton->setStyleSheet("background-color: lightgray");

    ui->redButton->setStyleSheet("background-color:darkred");
    ui->blueButton->setStyleSheet("background-color:darkblue");


    simonsTurn();
}

void MainWindow::on_redButton_clicked()
{
    compareButtonPress('A');
}

void MainWindow::on_blueButton_clicked()
{
    compareButtonPress('B');
}

void MainWindow::on_actionQuit_triggered()
{
    QApplication::quit();
}


/////////////////////
///    METHODS    ///
/////////////////////
void MainWindow::setGameOverScreen()
{
    ui->labelTurn->setText("Game Over!");

    ui->startButton->setEnabled(true);
    ui->startButton->setStyleSheet("background-color: chartreuse");

    ui->redButton->setEnabled(false);
    ui->redButton->setStyleSheet("background-color:lightgrey");

    ui->blueButton->setEnabled(false);
    ui->blueButton->setStyleSheet("background-color:lightgrey");

    ui->progressBar->setValue(0);
    game.initialize();//restart game
}

void MainWindow::simonsTurn()
{
    ui->labelTurn->setText("Simon's Turn");
    ui->redButton->setEnabled(false);
    ui->blueButton->setEnabled(false);
    wait(1500);
    unsigned long sequenceSize = game.getSequenceSize();
    for(unsigned long i = 0; i < sequenceSize; i++)
    {
        char currentButton = game.getNextButtonInSequence();
        if(currentButton == 'A')
        {
            flashRedButton();
        }
        else
        {
            flashBlueButton();
        }
    }

    game.resetSequenceIndex();

    ui->redButton->setEnabled(true);
    ui->blueButton->setEnabled(true);

    ui->labelTurn->setText("Your Turn");
    ui->progressBar->setValue(0);
}

void MainWindow::flashRedButton(){
    ui->redButton->setStyleSheet("background-color:red");
    wait(game.getFlashDelay());
    ui->redButton->setStyleSheet("background-color:darkred");
    wait(game.getFlashDelay() / 5);
}
void MainWindow::flashBlueButton(){
    ui->blueButton->setStyleSheet("background-color:blue");
    wait(game.getFlashDelay());
    ui->blueButton->setStyleSheet("background-color:darkblue");
    wait(game.getFlashDelay() / 5);
}

void MainWindow::wait(int time)
{
    QTime dieTime= QTime::currentTime().addMSecs(time);
    while (QTime::currentTime() < dieTime)
        QCoreApplication::processEvents(QEventLoop::AllEvents, 100);
}

void MainWindow::compareButtonPress(char c)
{
    int progress = 0;
    if(c != game.getNextButtonInSequence())
    {
        setGameOverScreen();
    }
    else
    {
        progress = game.turnProgressPercentage();
        ui->progressBar->setValue(progress);
    }

    if(progress == 100)
    {
        game.resetSequenceIndex();
        game.addRandomButtonToSequence();
        simonsTurn();
    }
}

void MainWindow::keyPressEvent(QKeyEvent * e){
    if(e->key()==Qt::Key_Left){
        ui->redButton->animateClick();
    }
    if(e->key()==Qt::Key_Right){
        ui->blueButton->animateClick();
    }
    if(e->key()==Qt::Key_Enter || e->key()==Qt::Key_Return){
        ui->startButton->animateClick();
    }
}

