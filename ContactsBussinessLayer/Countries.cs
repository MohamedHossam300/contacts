using System;
using System.Data;
using CountriesDataAccessLayer;

namespace CountriesBusinessLayer
{
    public class Country
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }

        enum enMode : byte
        {
            Update, Add
        }

        enMode _mode;

        public Country(string Name, string Code, string PhoneCode)
        {
            this.ID = -1;
            this.Name = Name;
            this.Code = Code;
            this.PhoneCode = PhoneCode;
            _mode = enMode.Add;
        }

        private Country(int ID, string Name, string Code, string PhoneCode) : this(Name, Code, PhoneCode)
        {
            this.ID = ID;
            _mode = enMode.Update;
        }

        static public Country Find(string CountryName)
        {
            CountriesDataAccess.stCountry searshed = new CountriesDataAccess.stCountry();
            if (CountriesDataAccess.Find(CountryName, ref searshed))
            {
                return new Country(searshed.ID, searshed.Name, searshed.Code, searshed.PhoneCode);
            }
            else
            {
                return null;
            }
        }

        static public Country Find(int CountryID)
        {
            CountriesDataAccess.stCountry searshed = new CountriesDataAccess.stCountry();
            if (CountriesDataAccess.Find(CountryID, ref searshed))
            {
                return new Country(searshed.ID, searshed.Name, searshed.Code, searshed.PhoneCode);
            }
            else
            {
                return null;
            }
        }

        public enum enSaveState : byte
        {
            Success,
            failedToSave,
            failedToUpdate,
            InvalidMode
        }

        bool _Add()
        {
            CountriesDataAccess.stCountry c = new CountriesDataAccess.stCountry(-1, this.Name, this.Code, this.PhoneCode);
            int index = CountriesDataAccess.Add(c);
            if (index != -1)
            {
                ID = index;
                return true;
            }

            return false;
        }

        bool _Update()
        {
            CountriesDataAccess.stCountry c = new CountriesDataAccess.stCountry(this.ID, this.Name, this.Code, this.PhoneCode);
            return CountriesDataAccess.Update(c);
        }

        public enSaveState Save()
        {
            switch (_mode)
            {
                case enMode.Add:
                    if (_Add())
                    {
                        _mode = enMode.Update;
                        return enSaveState.Success;
                    }
                    else
                        return enSaveState.failedToSave;
                case enMode.Update:
                    if (_Update())
                        return enSaveState.Success;
                    else
                        return enSaveState.failedToUpdate;
                default:
                    return enSaveState.InvalidMode;

            }
        }

        static public bool Delete(int id)
        {
            return CountriesDataAccess.Delete(id);
        }

        static public bool Delete(string Name)
        {
            return CountriesDataAccess.Delete(Name);
        }

        static public bool isFound(int id)
        {
            return CountriesDataAccess.isFound(id);
        }

        static public bool isFound(string Name)
        {
            return CountriesDataAccess.isFound(Name);
        }

        static public DataTable GetAllCoutries()
        {
            return CountriesDataAccess.GetAllCountries();
        }
    }
}
