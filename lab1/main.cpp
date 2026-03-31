#include <iostream>
#include <vector>
#include "Drink.h"
#include "Tea.h"
#include "Coffee.h"
using namespace std;

int main() {
	vector<Drink*> drinks;
	int с;
	
	Tea defaultT; 
	defaultT.Info();

	Coffee customC("капучино", 250, 250, 3, true);
	customC.Info();
	
	Tea copyTea(defaultT);
	copyTea.Info();
	
	drinks.push_back(new Tea("Зелёный чай", 250, 150, "зеленый", false));
	drinks.push_back(new Coffee("Латте", 400, 250, 3, true));
	drinks.push_back(new Tea(defaultT));
	drinks.push_back(new Coffee(customC));

	cout << "\n\n\n";

	static_cast<Coffee*>(drinks[1])->AddMilk();
	drinks[1]->Info();
	drinks[1]->GetWishes();

	cout << "\n\n\n";

	do {
		cout << "\nКАТАЛОГ НАПИТКОВ" << endl;
		for (int i = 0; i < drinks.size(); i++) {
			cout << i+1 << ". ";
			drinks[i]->Info();
		}

		cout << "0. Выход" << endl;
		cout << "Выберите напиток для настройки: ";
		cin >> с;

		if (с > 0 && с <= drinks.size()) {
			cout << "\nНастройка напитка" << endl;
			drinks[с-1]->GetWishes(); 
			cout << "Название: " << drinks[с-1]->GetName() << endl;
			cout << "Объём: " << drinks[с-1]->GetVolume() << "мл" << endl;
			cout << "Цена: " << drinks[с-1]->GetPrice() << "руб" << endl;

			int newVol;
		    cout << "Введите новый объём: ";
		    cin >> newVol;
		    drinks[с-1]->SetVolume(newVol);
		}
	} while (с != 0);
		
	return 0;
}