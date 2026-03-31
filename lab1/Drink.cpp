#include "Drink.h"
#include <iostream>
using namespace std;

// конструктор
Drink::Drink(string name, int volume, int price){
	this->name = name; 
	this->volume = volume;
	this->price = price;
	// this->name – это поле
	// name – это параметр
	// иначе компилятор не видит разницы
}


// геттеры
string Drink::GetName(){
	return name;
}

int Drink::GetVolume(){
	return volume;
}

int Drink::GetPrice(){
	return price;
}


// сеттеры
void Drink::SetName(string n){
	name = n;
}

void Drink::SetVolume(int v){
	volume = (v > 0) ? v : (cout << "Ошибка, объём должен быть больше 0" << endl, volume);
}

void Drink::SetPrice(int p){
	price = (p > 0) ? p : (cout << "Ошибка, цена должна быть больше 0" << endl, price);
}


// методы
void Drink::Info(){
	cout << "Название напитка: " << name << ", объём: " 
		 << volume << "мл" << ", цена: " << price << endl;
}


// деструктор
Drink::~Drink(){
	cout << "\nУдаляем напиток " << GetName() << endl;
}