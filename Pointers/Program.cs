// ---Parametry---
int first = 15;
int second = 20;
Console.WriteLine(first + ", " + second); // 15, 20
// chceme prohodit jejich hodnoty
void Swap(int num1, int num2)
{
    int temp = num1;
    num1 = num2;
    num2 = temp;
}
Swap(first, second); // ocekavame 20, 15
Console.WriteLine(first + ", " + second); // ale dostavame opet 15, 20

// PROC?
// na zasobniku se vytvori lokalni kopie promennych, ktere jsou predany v parametru
//      a po ukonceni podprogramu jsou zniceny
//  takto jsou predavany elementarni datove typy (int, double, bool, ...)

// 1) parametry predame REFERENCÍ (nevytvori se lokalni kopie promenne,
//      ale odkazeme se primo na adresu v pameti, na ktere je dana promenna) 
//  => vsechny zmeny promenne uvnitr podprogramu se projevi i mimo podprogram
//  takto jsou standardne predavany pole, objekty a retezce (nemusime psat ref, prida se tam implicitne)
void SwapWithRef(ref int num1, ref int num2)
{
    int temp = num1;
    num1 = num2;
    num2 = temp;
}
SwapWithRef(ref first, ref second); // ocekavame 20, 15
Console.WriteLine(first + ", " + second); // dostavame 20, 15

// 2) reference pomoci klicoveho slova out (hodi se tam, kdy z podprogramu potrebujeme vratit
//      vetsi mnozstvi promennych)
int Powers(int x, out int x3, out int x4, out double xMinus2)
{
    x3 = (int)Math.Pow(x, 3);
    x4 = (int)Math.Pow(x, 4);
    xMinus2 = Math.Pow(x, -2);
    return x * x;
}
int b = 2;
int b2;
int b3; // promenne musi existovat predem
int b4;
double bMinus10;
b2 = Powers(b, out b3, out b4, out bMinus10);
Console.WriteLine(b2 + ", " + b3 + ", " + b4 + ", " + bMinus10);

// ---Ukazatele---
unsafe // nebezpecny kod (ma primy pristup k pameti)
{
    // address-of
    int num = 51; // pevna promenna
    // operator ziskani adresy & - vrati adresu, prevadi hodnotovy datovy typ na ukazatel
    int* pointer = &num; // ukazatel na promennou
    Console.WriteLine("Hodnota proměnné: " + num); // 51
    Console.WriteLine("Adresa proměnné: " + ((long)pointer).ToString("X")); // E3C0F7E650

    byte[] bytesArr = new byte[5] { 1, 9, 58, 100, 255 }; // dynamicka (pohybliva) promenna - objekty, pole
    for(int i = 0; i < bytesArr.Length; i++)
    {
        // adresu ziskame pomoci bloku fixed
        fixed (byte* ptr = &bytesArr[i])
        {
            Console.WriteLine("Adresa prvku " + i + ": " + ((long)ptr).ToString("X"));
        }
    }

    // operator neprimeho pristupu (operator dereference)
    //      - vraci obsah adresy, prevadi ukazatel na hodnotovy datovy typ

    // operator pristupu ke clenu ukazatele (->)
    // x->y <=> (*x).y
    GeoLocations geolocations;
    GeoLocations* geo = &geolocations;
    geo->Longitude = 20;
    geo->Latitude = 30;
    geo->Altitude = 50;
    Console.WriteLine(geo->ToString());
    Console.WriteLine(((long)geo).ToString("X"));
    Console.WriteLine(geolocations.Altitude);

    // operator pristupu k prvku ukazatele ([])
    char* pointerToChars = stackalloc char[123]; // stackalloc - vyhradi prostor v pameti na zasobniku
    for (int i = 65; i < 123; i++)
    {
        pointerToChars[i] = (char)i;
        Console.Write(pointerToChars[i]);
    }
    Console.WriteLine();
}

using (GeoLocations geo = new(100, 200, 300))
{
    Console.WriteLine(geo.ToString());
}
struct GeoLocations : IDisposable
{
    public int Longitude { get; set; }
    public int Latitude { get; set; }
    public int Altitude { get; set; }

    public GeoLocations(int longitude = 10, int latitude = 20, int altitude = 30)
    {
        Longitude = longitude;
        Latitude = latitude;
        Altitude = altitude;
        Console.WriteLine("Instance vytvořena.");
    }

    public void Dispose() // finalizacni metoda, slouzi k uklidu
    {
        GC.SuppressFinalize(this);
        Console.WriteLine("Instance zlikvidována.");
    }

    public override string ToString()
    {
        return Longitude + ", " + Latitude + ", " + Altitude;
    }
}