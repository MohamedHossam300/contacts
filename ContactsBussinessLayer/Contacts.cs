using ContactsDataAccessLayer;
using System.Data;

namespace ContactsBusinessLayer
{
    public class Contacts
    {
        public int ID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }

        enum enMode : byte { eAdd, eUpdate }
        enMode _mode;

        public Contacts(string FirstName, string LastName, string Email, string Phone, string Address, int CountryID)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.CountryID = CountryID;
            _mode = enMode.eAdd;
        }

        private Contacts(int ID, string FirstName, string LastName, string Email, string Phone, string Address, int CountryID) : this(FirstName, LastName, Email, Phone, Address, CountryID)
        {
            this.ID = ID;
            _mode = enMode.eUpdate;
        }

        private Contacts(ContactsDataAccess.stContact contact) : this(contact.ID, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address, contact.CountryID)
        {
        }

        static public Contacts Find(int ID)
        {
            ContactsDataAccess.stContact search = new ContactsDataAccess.stContact();
            if (ContactsDataAccess.Find(ID, ref search))
            {
                return new Contacts(search);
            }
            else
            {
                return null;
            }
        }

        public enum enSaveStates : byte { eSuccess, eFailedToSave, eFailedToUpdate, eUnreachableMode }

        bool _AddContact()
        {
            return ((this.ID = ContactsDataAccess.Add(new ContactsDataAccess.stContact(-1, FirstName, LastName, Email, Phone, Address, CountryID))) != -1);
        }

        bool _UpdateContact()
        {
            return ContactsDataAccess.Update(new ContactsDataAccess.stContact(ID, FirstName, LastName, Email, Phone, Address, CountryID));
        }

        public enSaveStates Save()
        {
            switch (_mode)
            {
                case enMode.eAdd:
                    if (_AddContact())
                    {
                        _mode = enMode.eUpdate;
                        return enSaveStates.eSuccess;
                    }
                    else
                        return enSaveStates.eFailedToSave;

                case enMode.eUpdate:
                    if (_UpdateContact())
                        return enSaveStates.eSuccess;
                    else
                        return enSaveStates.eFailedToUpdate;
                default:
                    return enSaveStates.eUnreachableMode;
            }
        }

        static public bool Delete(int ID)
        {
            return ContactsDataAccess.Delete(ID);
        }

        static public DataTable GetAllContacts()
        {
            return ContactsDataAccess.GetAllContacts();
        }

        static public bool isFound(int id)
        {
            return ContactsDataAccess.isFound(id);
        }
    }
}
