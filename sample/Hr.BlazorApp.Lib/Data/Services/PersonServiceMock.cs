using Hr.BlazorApp.Lib.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace Hr.BlazorApp.Lib.Data.Services {
    public class PersonServiceMock : IPersonService {

        private static readonly ConcurrentDictionary<int, Person> _persons =
            new ConcurrentDictionary<int, Person>();

        static PersonServiceMock() {
            PopulateMockData();
        }


        public async Task<Person> CreateAsync(Person person) {
            Person NewPerson() => new Person {
                Id = _persons.Min(p => p.Key) - 1,
                DateOfBirth = person.DateOfBirth,
                FirstName = person.FirstName,
                LastName = person.LastName,
                SysStart = DateTime.Now,
                SysEnd = DateTime.MaxValue,
                SysUser = "tester@example.org"
            };
            var newPerson = NewPerson();
            await Task.Run(() => {
                while (!_persons.TryAdd(newPerson.Id, newPerson)) {
                    Thread.Sleep(100);
                    newPerson = NewPerson();
                }
            });
            return newPerson;
        }

        public async Task DeleteAsync(int id) {
            await Task.Run(() => {
                _persons.TryRemove(id, out _);
            });
        }

        public async Task<Person> GetAsync(int id) {
            var person = await Task.Run(() => {
                if (_persons.TryGetValue(id, out Person person))
                    return person;
                else
                    return null;
            });
            return person;
        }

        public async Task<PagedResult> GetPageAsync(string where = null, string orderBy = null, int? currentPage = 1, int? pageSize = 20, int? totalRecords = null) {
            IQueryable qry = _persons.Values.AsQueryable();
            if (where != null)
                qry = qry.Where(where);
            if (orderBy != null)
                qry = qry.OrderBy(orderBy);
            var pagedResult = await Task.Run(()=> 
                qry.PageResult(currentPage.Value, pageSize ?? 20));
            return pagedResult;
            ;
        }

        public async Task<Person> UpdateAsync(Person person) {
            Person NewPerson() => new Person {
                Id = person.Id,
                DateOfBirth = person.DateOfBirth,
                FirstName = person.FirstName,
                LastName = person.LastName,
                SysStart = DateTime.Now,
                SysEnd = DateTime.MaxValue,
                SysUser = "tester@example.org"
            };
            var newPerson = NewPerson();
            await Task.Run(() => {
                while (!_persons.TryUpdate(newPerson.Id, newPerson, _persons[newPerson.Id])) {
                    Thread.Sleep(100);
                    newPerson = NewPerson();
                }
            });
            return newPerson;
        }


        #region Mock Data
        private static void PopulateMockData() {

            Task.Run(() => {
                _persons.TryAdd(-999001, new Person { Id = -999001, FirstName = "Justino", LastName = "Castille", SysUser = "Castille", DateOfBirth = new DateTime(1964, 2, 23) });
                _persons.TryAdd(-999002, new Person { Id = -999002, FirstName = "Ruperto", LastName = "Gathwaite", SysUser = "Gathwaite", DateOfBirth = new DateTime(1967, 8, 7) });
                _persons.TryAdd(-999003, new Person { Id = -999003, FirstName = "Sile", LastName = "Wykes", SysUser = "Wykes", DateOfBirth = new DateTime(1953, 6, 26) });
                _persons.TryAdd(-999004, new Person { Id = -999004, FirstName = "Bil", LastName = "Danielou", SysUser = "Danielou", DateOfBirth = new DateTime(1980, 10, 1) });
                _persons.TryAdd(-999005, new Person { Id = -999005, FirstName = "Jonis", LastName = "Calder", SysUser = "Calder", DateOfBirth = new DateTime(1959, 12, 31) });
                _persons.TryAdd(-999006, new Person { Id = -999006, FirstName = "Emmanuel", LastName = "Ellett", SysUser = "Ellett", DateOfBirth = new DateTime(1946, 3, 23) });
                _persons.TryAdd(-999007, new Person { Id = -999007, FirstName = "Cristie", LastName = "Luppitt", SysUser = "Luppitt", DateOfBirth = new DateTime(1982, 11, 20) });
                _persons.TryAdd(-999008, new Person { Id = -999008, FirstName = "Letta", LastName = "Boscott", SysUser = "Boscott", DateOfBirth = new DateTime(1982, 10, 27) });
                _persons.TryAdd(-999009, new Person { Id = -999009, FirstName = "Cindra", LastName = "Pitman", SysUser = "Pitman", DateOfBirth = new DateTime(1951, 3, 20) });
                _persons.TryAdd(-999010, new Person { Id = -999010, FirstName = "Katrinka", LastName = "Alcide", SysUser = "Alcide", DateOfBirth = new DateTime(1955, 9, 17) });
                _persons.TryAdd(-999011, new Person { Id = -999011, FirstName = "Nicol", LastName = "Portigall", SysUser = "Portigall", DateOfBirth = new DateTime(1967, 12, 2) });
                _persons.TryAdd(-999012, new Person { Id = -999012, FirstName = "Pamela", LastName = "Woolnough", SysUser = "Woolnough", DateOfBirth = new DateTime(1956, 10, 24) });
                _persons.TryAdd(-999013, new Person { Id = -999013, FirstName = "Annabal", LastName = "Divine", SysUser = "Divine", DateOfBirth = new DateTime(1993, 6, 10) });
                _persons.TryAdd(-999014, new Person { Id = -999014, FirstName = "Rozella", LastName = "Hurdle", SysUser = "Hurdle", DateOfBirth = new DateTime(1971, 10, 2) });
                _persons.TryAdd(-999015, new Person { Id = -999015, FirstName = "Adelheid", LastName = "Kerby", SysUser = "Kerby", DateOfBirth = new DateTime(1972, 2, 11) });
                _persons.TryAdd(-999016, new Person { Id = -999016, FirstName = "Briant", LastName = "Beckles", SysUser = "Beckles", DateOfBirth = new DateTime(1995, 4, 26) });
                _persons.TryAdd(-999017, new Person { Id = -999017, FirstName = "Neils", LastName = "Ovise", SysUser = "Ovise", DateOfBirth = new DateTime(1952, 9, 4) });
                _persons.TryAdd(-999018, new Person { Id = -999018, FirstName = "Giffie", LastName = "Krause", SysUser = "Krause", DateOfBirth = new DateTime(1999, 9, 11) });
                _persons.TryAdd(-999019, new Person { Id = -999019, FirstName = "Hannah", LastName = "Giovannoni", SysUser = "Giovannoni", DateOfBirth = new DateTime(1987, 5, 13) });
                _persons.TryAdd(-999020, new Person { Id = -999020, FirstName = "Bastien", LastName = "Buston", SysUser = "Buston", DateOfBirth = new DateTime(1998, 8, 27) });
                _persons.TryAdd(-999021, new Person { Id = -999021, FirstName = "Luca", LastName = "Wildey", SysUser = "Wildey", DateOfBirth = new DateTime(1976, 6, 2) });
                _persons.TryAdd(-999022, new Person { Id = -999022, FirstName = "Tyrus", LastName = "Cantor", SysUser = "Cantor", DateOfBirth = new DateTime(1985, 2, 7) });
                _persons.TryAdd(-999023, new Person { Id = -999023, FirstName = "Jerrie", LastName = "Harrison", SysUser = "Harrison", DateOfBirth = new DateTime(1977, 11, 18) });
                _persons.TryAdd(-999024, new Person { Id = -999024, FirstName = "Hyacinthia", LastName = "Dunlea", SysUser = "Dunlea", DateOfBirth = new DateTime(1984, 6, 13) });
                _persons.TryAdd(-999025, new Person { Id = -999025, FirstName = "Temp", LastName = "Cullingworth", SysUser = "Cullingworth", DateOfBirth = new DateTime(1992, 1, 6) });
                _persons.TryAdd(-999026, new Person { Id = -999026, FirstName = "Berne", LastName = "Whitnall", SysUser = "Whitnall", DateOfBirth = new DateTime(1963, 2, 6) });
                _persons.TryAdd(-999027, new Person { Id = -999027, FirstName = "Baillie", LastName = "Batiste", SysUser = "Batiste", DateOfBirth = new DateTime(1940, 1, 3) });
                _persons.TryAdd(-999028, new Person { Id = -999028, FirstName = "Clyve", LastName = "Le Brun", SysUser = "Le Brun", DateOfBirth = new DateTime(1968, 3, 16) });
                _persons.TryAdd(-999029, new Person { Id = -999029, FirstName = "Cherlyn", LastName = "Chevalier", SysUser = "Chevalier", DateOfBirth = new DateTime(1969, 11, 2) });
                _persons.TryAdd(-999030, new Person { Id = -999030, FirstName = "Torrance", LastName = "Wallbanks", SysUser = "Wallbanks", DateOfBirth = new DateTime(1968, 4, 29) });
                _persons.TryAdd(-999031, new Person { Id = -999031, FirstName = "Alexandro", LastName = "Shory", SysUser = "Shory", DateOfBirth = new DateTime(1972, 1, 22) });
                _persons.TryAdd(-999032, new Person { Id = -999032, FirstName = "Abdul", LastName = "Twiname", SysUser = "Twiname", DateOfBirth = new DateTime(1989, 5, 28) });
                _persons.TryAdd(-999033, new Person { Id = -999033, FirstName = "Nixie", LastName = "Chazier", SysUser = "Chazier", DateOfBirth = new DateTime(1944, 1, 10) });
                _persons.TryAdd(-999034, new Person { Id = -999034, FirstName = "Eran", LastName = "Pickton", SysUser = "Pickton", DateOfBirth = new DateTime(1950, 9, 13) });
                _persons.TryAdd(-999035, new Person { Id = -999035, FirstName = "Grace", LastName = "Kuhlen", SysUser = "Kuhlen", DateOfBirth = new DateTime(1945, 2, 27) });
                _persons.TryAdd(-999036, new Person { Id = -999036, FirstName = "Liv", LastName = "Callander", SysUser = "Callander", DateOfBirth = new DateTime(1970, 3, 26) });
                _persons.TryAdd(-999037, new Person { Id = -999037, FirstName = "Cristobal", LastName = "Kenshole", SysUser = "Kenshole", DateOfBirth = new DateTime(1949, 3, 23) });
                _persons.TryAdd(-999038, new Person { Id = -999038, FirstName = "Suzanne", LastName = "Matterdace", SysUser = "Matterdace", DateOfBirth = new DateTime(1976, 9, 18) });
                _persons.TryAdd(-999039, new Person { Id = -999039, FirstName = "Lanae", LastName = "Wiltsher", SysUser = "Wiltsher", DateOfBirth = new DateTime(1979, 5, 19) });
                _persons.TryAdd(-999040, new Person { Id = -999040, FirstName = "Donal", LastName = "Janssens", SysUser = "Janssens", DateOfBirth = new DateTime(1954, 1, 26) });
                _persons.TryAdd(-999041, new Person { Id = -999041, FirstName = "Dicky", LastName = "Carriage", SysUser = "Carriage", DateOfBirth = new DateTime(1950, 1, 28) });
                _persons.TryAdd(-999042, new Person { Id = -999042, FirstName = "Rora", LastName = "Bothe", SysUser = "Bothe", DateOfBirth = new DateTime(1997, 4, 8) });
                _persons.TryAdd(-999043, new Person { Id = -999043, FirstName = "Rhys", LastName = "Ranald", SysUser = "Ranald", DateOfBirth = new DateTime(1967, 10, 19) });
                _persons.TryAdd(-999044, new Person { Id = -999044, FirstName = "Tabbatha", LastName = "Buttle", SysUser = "Buttle", DateOfBirth = new DateTime(1973, 2, 2) });
                _persons.TryAdd(-999045, new Person { Id = -999045, FirstName = "Row", LastName = "Rayson", SysUser = "Rayson", DateOfBirth = new DateTime(1953, 2, 10) });
                _persons.TryAdd(-999046, new Person { Id = -999046, FirstName = "Thurston", LastName = "Robez", SysUser = "Robez", DateOfBirth = new DateTime(1970, 1, 3) });
                _persons.TryAdd(-999047, new Person { Id = -999047, FirstName = "Gaelan", LastName = "Thatcher", SysUser = "Thatcher", DateOfBirth = new DateTime(1966, 7, 26) });
                _persons.TryAdd(-999048, new Person { Id = -999048, FirstName = "Jamima", LastName = "Peegrem", SysUser = "Peegrem", DateOfBirth = new DateTime(1986, 9, 14) });
                _persons.TryAdd(-999049, new Person { Id = -999049, FirstName = "Mable", LastName = "Gatley", SysUser = "Gatley", DateOfBirth = new DateTime(1981, 6, 17) });
                _persons.TryAdd(-999050, new Person { Id = -999050, FirstName = "Wilden", LastName = "McKniely", SysUser = "McKniely", DateOfBirth = new DateTime(1984, 1, 2) });
                _persons.TryAdd(-999051, new Person { Id = -999051, FirstName = "Helyn", LastName = "Macallam", SysUser = "Macallam", DateOfBirth = new DateTime(1961, 5, 6) });
                _persons.TryAdd(-999052, new Person { Id = -999052, FirstName = "Letizia", LastName = "McMurraya", SysUser = "McMurraya", DateOfBirth = new DateTime(1982, 2, 6) });
                _persons.TryAdd(-999053, new Person { Id = -999053, FirstName = "Eugenie", LastName = "Dabes", SysUser = "Dabes", DateOfBirth = new DateTime(1944, 5, 1) });
                _persons.TryAdd(-999054, new Person { Id = -999054, FirstName = "Rosaleen", LastName = "Bunney", SysUser = "Bunney", DateOfBirth = new DateTime(1947, 2, 13) });
                _persons.TryAdd(-999055, new Person { Id = -999055, FirstName = "Norby", LastName = "Swanton", SysUser = "Swanton", DateOfBirth = new DateTime(1988, 7, 29) });
                _persons.TryAdd(-999056, new Person { Id = -999056, FirstName = "Maddalena", LastName = "Dederich", SysUser = "Dederich", DateOfBirth = new DateTime(1949, 1, 12) });
                _persons.TryAdd(-999057, new Person { Id = -999057, FirstName = "Burlie", LastName = "Mayhow", SysUser = "Mayhow", DateOfBirth = new DateTime(1967, 10, 18) });
                _persons.TryAdd(-999058, new Person { Id = -999058, FirstName = "Raymund", LastName = "Garrod", SysUser = "Garrod", DateOfBirth = new DateTime(1973, 5, 28) });
                _persons.TryAdd(-999059, new Person { Id = -999059, FirstName = "Moises", LastName = "Stubbeley", SysUser = "Stubbeley", DateOfBirth = new DateTime(1973, 2, 15) });
                _persons.TryAdd(-999060, new Person { Id = -999060, FirstName = "Rosabella", LastName = "Simo", SysUser = "Simo", DateOfBirth = new DateTime(1960, 1, 2) });
                _persons.TryAdd(-999061, new Person { Id = -999061, FirstName = "Yorgo", LastName = "Andreopolos", SysUser = "Andreopolos", DateOfBirth = new DateTime(1949, 9, 23) });
                _persons.TryAdd(-999062, new Person { Id = -999062, FirstName = "Elvin", LastName = "Canavan", SysUser = "Canavan", DateOfBirth = new DateTime(1965, 2, 26) });
                _persons.TryAdd(-999063, new Person { Id = -999063, FirstName = "Norton", LastName = "Lob", SysUser = "Lob", DateOfBirth = new DateTime(1984, 3, 1) });
                _persons.TryAdd(-999064, new Person { Id = -999064, FirstName = "Martin", LastName = "Roos", SysUser = "Roos", DateOfBirth = new DateTime(1963, 4, 9) });
                _persons.TryAdd(-999065, new Person { Id = -999065, FirstName = "Gare", LastName = "Fearne", SysUser = "Fearne", DateOfBirth = new DateTime(1997, 10, 24) });
                _persons.TryAdd(-999066, new Person { Id = -999066, FirstName = "Courtney", LastName = "Paynton", SysUser = "Paynton", DateOfBirth = new DateTime(1994, 8, 16) });
                _persons.TryAdd(-999067, new Person { Id = -999067, FirstName = "Duane", LastName = "Antoniazzi", SysUser = "Antoniazzi", DateOfBirth = new DateTime(1988, 1, 22) });
                _persons.TryAdd(-999068, new Person { Id = -999068, FirstName = "Jarrid", LastName = "Boumphrey", SysUser = "Boumphrey", DateOfBirth = new DateTime(1961, 2, 19) });
                _persons.TryAdd(-999069, new Person { Id = -999069, FirstName = "Giovanni", LastName = "Devericks", SysUser = "Devericks", DateOfBirth = new DateTime(1981, 4, 9) });
                _persons.TryAdd(-999070, new Person { Id = -999070, FirstName = "Katrine", LastName = "Larimer", SysUser = "Larimer", DateOfBirth = new DateTime(1990, 10, 1) });
                _persons.TryAdd(-999071, new Person { Id = -999071, FirstName = "Hedwiga", LastName = "McDermott-Row", SysUser = "McDermott-Row", DateOfBirth = new DateTime(1941, 7, 26) });
                _persons.TryAdd(-999072, new Person { Id = -999072, FirstName = "Myranda", LastName = "Godwyn", SysUser = "Godwyn", DateOfBirth = new DateTime(1962, 7, 7) });
                _persons.TryAdd(-999073, new Person { Id = -999073, FirstName = "Loleta", LastName = "Dibbert", SysUser = "Dibbert", DateOfBirth = new DateTime(1985, 9, 26) });
                _persons.TryAdd(-999074, new Person { Id = -999074, FirstName = "Monty", LastName = "Gawthorpe", SysUser = "Gawthorpe", DateOfBirth = new DateTime(1963, 10, 31) });
                _persons.TryAdd(-999075, new Person { Id = -999075, FirstName = "Granny", LastName = "Fawcitt", SysUser = "Fawcitt", DateOfBirth = new DateTime(1976, 3, 24) });
                _persons.TryAdd(-999076, new Person { Id = -999076, FirstName = "Judon", LastName = "Brands", SysUser = "Brands", DateOfBirth = new DateTime(1941, 3, 19) });
                _persons.TryAdd(-999077, new Person { Id = -999077, FirstName = "Christin", LastName = "Letterese", SysUser = "Letterese", DateOfBirth = new DateTime(1974, 4, 10) });
                _persons.TryAdd(-999078, new Person { Id = -999078, FirstName = "Karola", LastName = "Tother", SysUser = "Tother", DateOfBirth = new DateTime(1966, 1, 28) });
                _persons.TryAdd(-999079, new Person { Id = -999079, FirstName = "Shirlee", LastName = "Berge", SysUser = "Berge", DateOfBirth = new DateTime(1992, 7, 5) });
                _persons.TryAdd(-999080, new Person { Id = -999080, FirstName = "Jammal", LastName = "Orcas", SysUser = "Orcas", DateOfBirth = new DateTime(1995, 6, 19) });
                _persons.TryAdd(-999081, new Person { Id = -999081, FirstName = "Hiram", LastName = "Meininger", SysUser = "Meininger", DateOfBirth = new DateTime(1996, 5, 20) });
                _persons.TryAdd(-999082, new Person { Id = -999082, FirstName = "Tadd", LastName = "Ducker", SysUser = "Ducker", DateOfBirth = new DateTime(1987, 10, 28) });
                _persons.TryAdd(-999083, new Person { Id = -999083, FirstName = "Tiphanie", LastName = "Drinkhill", SysUser = "Drinkhill", DateOfBirth = new DateTime(1976, 12, 3) });
                _persons.TryAdd(-999084, new Person { Id = -999084, FirstName = "Nanete", LastName = "Waterdrinker", SysUser = "Waterdrinker", DateOfBirth = new DateTime(1954, 4, 4) });
                _persons.TryAdd(-999085, new Person { Id = -999085, FirstName = "Cos", LastName = "Lattin", SysUser = "Lattin", DateOfBirth = new DateTime(1975, 2, 28) });
                _persons.TryAdd(-999086, new Person { Id = -999086, FirstName = "Alden", LastName = "Braddock", SysUser = "Braddock", DateOfBirth = new DateTime(1954, 2, 27) });
                _persons.TryAdd(-999087, new Person { Id = -999087, FirstName = "Karoline", LastName = "Passie", SysUser = "Passie", DateOfBirth = new DateTime(1995, 12, 12) });
                _persons.TryAdd(-999088, new Person { Id = -999088, FirstName = "Hannah", LastName = "Burroughes", SysUser = "Burroughes", DateOfBirth = new DateTime(1964, 6, 22) });
                _persons.TryAdd(-999089, new Person { Id = -999089, FirstName = "Pepito", LastName = "Elmes", SysUser = "Elmes", DateOfBirth = new DateTime(1953, 9, 18) });
                _persons.TryAdd(-999090, new Person { Id = -999090, FirstName = "Dee", LastName = "Panter", SysUser = "Panter", DateOfBirth = new DateTime(1994, 8, 13) });
                _persons.TryAdd(-999091, new Person { Id = -999091, FirstName = "Flossi", LastName = "Waller-Bridge", SysUser = "Waller-Bridge", DateOfBirth = new DateTime(1984, 8, 5) });
                _persons.TryAdd(-999092, new Person { Id = -999092, FirstName = "Juditha", LastName = "Tenby", SysUser = "Tenby", DateOfBirth = new DateTime(1981, 5, 25) });
                _persons.TryAdd(-999093, new Person { Id = -999093, FirstName = "Walliw", LastName = "Rooksby", SysUser = "Rooksby", DateOfBirth = new DateTime(1950, 5, 28) });
                _persons.TryAdd(-999094, new Person { Id = -999094, FirstName = "Lindon", LastName = "Gunson", SysUser = "Gunson", DateOfBirth = new DateTime(1960, 5, 11) });
                _persons.TryAdd(-999095, new Person { Id = -999095, FirstName = "Minna", LastName = "Sawney", SysUser = "Sawney", DateOfBirth = new DateTime(1952, 1, 15) });
                _persons.TryAdd(-999096, new Person { Id = -999096, FirstName = "Killian", LastName = "Cowderoy", SysUser = "Cowderoy", DateOfBirth = new DateTime(1970, 6, 8) });
                _persons.TryAdd(-999097, new Person { Id = -999097, FirstName = "Raynell", LastName = "Ettritch", SysUser = "Ettritch", DateOfBirth = new DateTime(1986, 6, 15) });
                _persons.TryAdd(-999098, new Person { Id = -999098, FirstName = "Kesley", LastName = "Laybourn", SysUser = "Laybourn", DateOfBirth = new DateTime(1982, 8, 11) });
                _persons.TryAdd(-999099, new Person { Id = -999099, FirstName = "Deina", LastName = "Jolin", SysUser = "Jolin", DateOfBirth = new DateTime(1988, 8, 6) });
                _persons.TryAdd(-999100, new Person { Id = -999100, FirstName = "Anne-marie", LastName = "Towse", SysUser = "Towse", DateOfBirth = new DateTime(1969, 10, 5) });

            });
        }
        #endregion
    }
}
