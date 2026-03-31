#include "Tea.h"
#include <iostream>
using namespace std;

// конструкторы
//по-умолчанию
Tea::Tea() : Drink("Чай", 250, 100), type("черный"), hasLemon(false) {}

//основной
Tea::Tea(string name, int volume, int price, string type, bool hasLemon)
: Drink(name, volume, price) { // вызов конструктора родителя
	this->type = type;
	this->hasLemon = hasLemon;
}

//копирования
Tea::Tea(const Tea &t) : Drink(t), type(t.type), hasLemon(t.hasLemon) {} 


// геттеры
string Tea::GetType(){
	return type;
}

bool Tea::GetHasLemon(){
	return hasLemon;
}

// сеттеры
void Tea::SetType(string type){
	this->type = type;
}

void Tea::SetHasLemon(bool hasLemon){
	this->hasLemon = hasLemon;
}

// доп методы
void Tea::AddLemon(){
	hasLemon = true;
	cout << "Лимон добавлен в чай!!!" << endl;
}


void Tea::RemoveLemon(){
	hasLemon = false;
	cout << "Лимона больше нет в чае :(" << endl;
}

// переопределённые методы
void Tea::Info(){
	cout << "Название: " << GetName()
	<< ", объём: " << GetVolume() << endl;
}

void Tea::GetWishes() {
    cout << "Добавление чая" << endl;
    
    int tc;
    cout << "Выберите тип чая:" << endl;
    cout << "1. Зелёный" << endl;
    cout << "2. Чёрный" << endl;
    cout << "3. Травяной" << endl;
    cout << "Ваш выбор: ";
    cin >> tc;
    
    if (tc == 1) type = "зелёный";
    else if (tc == 2) type = "чёрный";
    else if (tc == 3) type = "травяной";
    
    int lemon;
    cout << "Добавить лимон? (1 - да, 0 - нет): " << endl;
    cin >> lemon;
    hasLemon = (lemon == 1);
        
    cout << "Ваш чай готов:" << endl;
}
