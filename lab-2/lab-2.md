# Lab 2 - HTML/Binding

**Predaja: Četvrtak 17.4.2026.**

## Zadaci i bodovanje

| Kriterij | Bodovi |
| --- | --- |
| Prompt za sub-agenta za UI/UX | 1 |
| Log da je sub-agent pozivan za UI/UX | 1 |
| Napravljen unique UX (non standard) koji radi s mock repository-ima | 2 |
| Usmeno ispitivanje razumjevanja rada s custom agentima | 1 |
- [ ]  Sav kod treba biti na GH repozitoriju, default branch
    - [ ]  Kreirati custom agenta (sub-agent) za UX — definirati prompt koji opisuje kako bi UX trebao biti rađen (stil, komponente, layout principi). Osigurati da je agent instruction file commited na Git.
- [ ]  Osigurati da glavni agent spawna UX sub-agenta pri generiranju UI koda — potreban je log kao dokaz
- [ ]  Koristiti mock repository sa statičkim podacima iz [Lab 1 - Osnove C# / LINQ](https://www.notion.so/Lab-1-Osnove-C-LINQ-322ce226e51f8011bea7d653c9e64048?pvs=21) (objektni model i popunjeni podaci)
    - [ ]  Implementirati sve stranice za prikaz podataka (Index/lista) za svaki entitet — bez Create/Edit opcija
    - [ ]  Implementirati stranice za prikaz detalja (Details) za svaki entitet
    - [ ]  Implementirati specifičnu stranicu - custom home page ili slično (primjer s predavanja je bila stranica za rješavanje kviza)
    - [ ]  Implementirati kompletnu navigaciju između svih stranica (izbornik, linkovi s liste na detalje, breadcrumbs)
- [ ]  UX mora biti unique/non-standard (ne default Bootstrap template)
- [ ]  Pripremiti se za usmeno ispitivanje organizacije UI/UX i kako promptati agente

---

### Nomenklatura i konvencije

Na primjeru `HomeController` controllera i `About` metode:

- U folderu `Controllers`, nalazi se `HomeController`. Svaki kontroler završava sufixom "Controller".
- `HomeController` sadrži nekoliko akcija – promotrimo akciju `About`.
- Akcija `About` vraća `return View()`. O kojem točno view-u se radi, povezuje se preko naziva akcije. Konkretno, iz foldera `Views/Home` se pokušava locirati .cshtml datoteka istog imena kao i akcija – u ovom slučaju `About.cshtml`.
- Struktura Views odgovara strukturi kontrolera – folderi `Home`, `Manage`, `Account` su upravo nazivi kontrolera iz mape Controllers.

# Twitter Bootstrap

Ili popularnije, samo bootstrap, je skup javascript i CSS biblioteka koje omogućavaju lakši razvoj web aplikacija, pri tome inicijalno postavljajući dobar dizajn i pružajući niz funkcionalnosti.

Kompletna dokumentacija s nizom primjera: [http://getbootstrap.com/](http://getbootstrap.com/)

## Grid system

Ideja je da se cijelo korisničko sučelje podijeli u mrežu manjih dijelova, koji se dalje dijele opet u mrežu, itd. Bitno svojstvo ovakve mreže je da je (manje-više) automatski prilagodljivo veličini ekrana (mobiteli, tableti, široki ekrani) te na taj način drastično poboljšava iskustvo korisnika.

Detalji: [https://getbootstrap.com/docs/5.1/layout/grid/](https://getbootstrap.com/docs/5.1/layout/grid/)

## Modal

Vrlo često korištena komponenta za prikaz informacija u popup prozoru. Uz mogućnost otvaranja popup prozora samo korištenjem HTML-a, dodatno omogućava i proširenja korištenjem javascript funkcija.

Dokumentacija: [https://getbootstrap.com/docs/5.1/components/modal/](https://getbootstrap.com/docs/5.1/components/modal/)

## Details – pregled detalja

Uz listu, prilagođeni pregled detalja se također veoma često koristi u poslovnim
aplikacijama. Na Index stranici se prikazuju jednostavniji/kompaktniji podaci,
dok se na stranici detalja prikazuju detaljniji podaci – u kontekstu aplikacije
za evidenciju klijenata, to bi mogli biti slika, ime, prezime, adresa, sumarni
podaci o sastancima, popis sastanaka, itd.

# Mock repository i dependency injection

U ovoj vježbi podaci se ne dohvaćaju iz prave baze, nego iz **mock repository**
klasa koje vraćaju statičke podatke. Time je moguće vrlo brzo razviti i
demonstrirati aplikaciju, a kasnije bez velikih promjena zamijeniti mock
implementaciju pravom bazom podataka.

## Mock repository

Tipičan pristup je da za svaki glavni entitet postoji zasebna klasa koja zna
vratiti podatke za taj entitet. Za Lab 2 to znači da se podaci iz objektnog
modela i napunjenih instanci iz Lab 1 mogu preseliti u repozitorije kao statički
ili unaprijed pripremljeni podaci.

Najčešće metode koje takav repozitorij ima su:

- `GetAll()` – vraća sve zapise potrebne za Index/lista stranicu
- `GetById(int id)` – vraća jedan konkretan zapis za Details stranicu

Primjeri naziva klasa:

- `AuthorMockRepository`
- `QuizMockRepository`

Prednost ovakvog pristupa je da controller ne mora znati *odakle* podaci dolaze.
Njegov je posao samo tražiti podatke i proslijediti ih view-u.

## Dependency injection

U modernim .NET aplikacijama uobičajeno je da se ovisnosti ne kreiraju ručno pomoću `new` unutar controllera, nego se registriraju u `Program.cs`, a framework ih zatim automatski prosljeđuje kroz konstruktor controllera.

Primjer registracije mock repository klasa:

```csharp
builder.Services.AddSingleton<AuthorMockRepository>();
builder.Services.AddSingleton<QuizMockRepository>();
```

Primjer korištenja u controlleru:

```csharp
public class AuthorController : Controller
{
    private readonly AuthorMockRepository _authorRepository;

    public AuthorController(AuthorMockRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public IActionResult Index()
    {
        var authors = _authorRepository.GetAll();
        return View(authors);
    }
}
```

Na ovaj način controller ostaje jednostavan:

- prima ovisnosti kroz konstruktor
- dohvaća podatke iz repository klase
- šalje podatke odgovarajućem view-u

Kasnije se mock repository može zamijeniti pravim repositoryjem ili servisom bez velikih promjena u controllerima.

# Model binding

Model binding je jedan od osnovnih koncepata MVC paradigme.

## Povezivanje vrijednosti forme i parametara akcije

### Pristup 1: FormCollection (najlošiji)

```csharp
[HttpPost]
public ActionResult Contact(FormCollection formData)
{
    var ime = formData["ime"];
    var prezime = formData["prezime"];
    return View();
}
```

Problemi:

- Velika mogućnost pogreške jer se sva imena prenose preko stringova
- Potrebno je pisati vlastiti kod za pretvorbu raznih tipova
- Posebni načini rukovanja bool vrijednostima

### Pristup 2: Jednostavni parametri (bolji)

```csharp
[HttpPost]
public ActionResult Contact(string ime, string prezime)
{
    //Obrada podataka
    return View();
}
```

Preko atributa `name` u HTML jeziku se definira naziv tog parametra u akciji.
Ovo je bolje, ali i dalje: broj parametara u metodi može drastično rasti i nije
se smanjila mogućnost pogreške krivog naziva.

### Pristup 3: Model binding (najbolji)

Moguće je ostaviti [ASP.NET](http://ASP.NET) MVC-u da automatski popuni polja
u objektu prema imenima HTML input polja. Ukoliko se kao parametar akcije
očekuje kompleksni objekt (vlastito kreirani model), [ASP.NET](http://ASP.NET)
MVC mehanizam će pokušati kreirati instancu tog objekta, te popuniti njegova
svojstva s obzirom na imena polja na formi:

```csharp
public class ContactModel
{
    public string Ime { get; set; }
    public string Prezime { get; set; }
}
```

```html
<form action="/Home/Contact" method="post">
    <div>
        Ime: <input type="text" name="ime" />
        Prezime: <input type="text" name="prezime" />
    </div>
</form>
```

```csharp
[HttpPost]
public ActionResult Contact(ContactModel model)
{
    var ime = model.Ime;
    var prezime = model.Prezime;
    return View();
}
```

### Pristup 3b: Razor EditorFor (najtipskiji)

Kako bi riješili problem mogućnosti pogreške u nazivu parametara, koristimo
razor funkcionalnost koja se bazira na stablima izraza kako bi se napravilo HTML
input polje na način da se specifično povezuje sa željenim poljem u modelu:

```csharp
@model QuizManager.Web.Models.ContactModel

<section class="contact-form">
    <header>
        <h3>Pošaljite nam upit!</h3>
        <form action="/Home/Contact" method="post" class="form-inline">
            <div>
                Ime: @Html.EditorFor(p => p.Ime, new
                    { htmlAttributes = new {
                        @class = "form-control",
                        placeholder = "Pretraga po nazivu" }
                    })
                Prezime: <input type="text" name="prezime" />
            </div>
```

Gornji način se može koristiti samo u slučaju da polja odgovaraju direktno
poljima modela, stoga je prvo potrebno razumjeti koncept korisničke
kontrole/djelomičnog pogleda (partial view) kako bi se ova funkcionalnost mogla
implementirati na ekran za pregled/pretragu klijenata.
