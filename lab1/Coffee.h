#ifndef COFFEE_H
#define COFFEE_H
using namespace std;
#include <iostream>
#include <string>
#include "Drink.h" // класс напитков (родитель)

class Coffee : public Drink {
private:
	int strength; // крепость кофе от 1 до 5
	bool hasMilk; //наличие молока

public:
	//конструкторы
	Coffee(); //конструктор по-умолчанию
	Coffee(string name, int volume, int price, int strength, bool hasMilk); //основной
	Coffee(const Coffee &c); //конструктор копирования


	// геттеры
	int GetStrength();
	bool GetHasMilk();

	// сеттеры
	void SetStrength(int strength);
	void SetHasMilk(bool hasMilk);

	// доп методы
	void AddMilk();
	void RemoveMilk();

	// переопределенные методы 
	void GetWishes() override;
	void Info() override;
 	// override - переопределить
};

#endif