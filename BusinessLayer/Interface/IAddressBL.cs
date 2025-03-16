using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IAddressBL
    {
        public List<AddressBookModel> GetAllAddress();
        public AddressBookModel GetAddressByID(int id);
        public AddContactModel AddContact(AddContactModel newContact);
        public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact);
        public bool DeleteContact(int id);
    }
}