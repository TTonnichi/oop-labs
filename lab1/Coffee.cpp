#include "Coffee.h"
#include <iostream>
using namespace std;

// конструкторы
Coffee::Coffee() : Drink("Кофе", 200, 150), strength(3), hasMilk(false) {}

Coffee::Coffee(string name, int volume, int price, int  strength, bool hasMilk)
: Drink(name, volume, price) { // вызов конструктора родителя
	this->strength = strength;
	this->hasMilk = hasMilk;
}

Coffee::Coffee(const Coffee &c) : Drink(c), strength(c.strength), hasMilk(c.hasMilk) {}


// геттеры
int Coffee::GetStrength() {return strength;}
bool Coffee::GetHasMilk() {return hasMilk;}

//сеттеры
void Coffee::SetStrength(int strength){this->strength = strength;}
void Coffee::SetHasMilk(bool hasMilk){this->hasMilk = hasMilk;}

// доп методы
void Coffee::AddMilk() {hasMilk = true; cout << "Молоко добавлено! " << endl;}
void Coffee::RemoveMilk() {hasMilk = false; cout << "Молока больше нет :(" << endl;}

// переопределенные
void Coffee::Info(){
	cout << "Название кофе: " << GetName()
		<< ", объём: " << GetVolume() << ", молоко: " << (hasMilk ? "есть" : "нет") << endl;
}

void Coffee::GetWishes() {
    cout << "\nДобавление кофеееее" << endl;
    
    cout << "Выберите крепость (1-5): ";
    cin >> strength;
    
    int milk;
    cout << "Добавить молоко? (1 - да, 0 - нет): ";
    cin >> milk;
    hasMilk = (milk == 1);
        
    int size;
    cout << "Напишите объём в мл:" << endl;
    cin >> size;
        
    cout << "\nВаш кофе готов:" << endl;
}