#ifndef DRINK_H
#define DRINK_H

#include <iostream>
#include <string>
using namespace std;

class Drink {
private:
	string name; // название
	int volume; // объем (в мл)
	int price; // цена 

public:
	// конструктор
	Drink(string name, int value, int price); 

	// геттеры (получение значений (get))
	string GetName();
	int GetVolume();
	int GetPrice();

	// сеттеры (установка значений (set))
 	void SetName(string n);
	void SetVolume(int v);
	void SetPrice(int p);

	// методы
	// что можно делать с напитками.....
	// допустим, готовка и... я не придумала
	virtual void GetWishes() = 0;
	virtual void Info();


	// деструктор (виртуальный)
	virtual ~Drink();

};

#endif