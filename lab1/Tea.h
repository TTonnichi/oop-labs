#ifndef TEA_H
#define TEA_H
using namespace std;
#include <iostream>
#include <string>
#include "Drink.h" // класс напитков (родитель)


class Tea : public Drink{
private:
	string type; // тип чая (чёрный там, зелёный, травяной)
	bool hasLemon; // наличие лимона

public:
	// конструкторы
	Tea();
	Tea(string name, int volume, int price, string type, bool hasLemon); 
	Tea(const Tea &t); // const - ориг не меняется, & - передача по ссылке, t - оригинал 


	// чайные геттеры
	string GetType();
	bool GetHasLemon();

	// чайные сеттеры
	void SetType(string type);
	void SetHasLemon(bool hasLemon);

	// дополнительные методы
	void AddLemon();	
	void RemoveLemon();

	// переопределенные методы 
	void GetWishes() override;
	void Info() override;
 	// override - переопределить

};


#endif
