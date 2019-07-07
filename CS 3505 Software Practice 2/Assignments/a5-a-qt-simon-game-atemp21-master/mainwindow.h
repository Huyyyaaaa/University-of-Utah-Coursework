#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "simonmodel.h"
#include <QKeyEvent>
#include <QDialog>

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

private: //fields
    Simon game;
    Ui::MainWindow *ui;

public: //constructors
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_startButton_clicked();
    void on_blueButton_clicked();
    void on_redButton_clicked();
    void on_actionQuit_triggered();

private: //methods
    void simonsTurn();
    void wait(int);
    void compareButtonPress(char c);
    void flashRedButton();
    void flashBlueButton();
    void setGameOverScreen();

protected:
    void keyPressEvent(QKeyEvent *);
};

#endif // MAINWINDOW_H
