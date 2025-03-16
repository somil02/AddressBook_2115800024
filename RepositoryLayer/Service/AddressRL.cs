using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class AddressRL : IAddressRL
    {
        AddressBookDbContext _dbContext;
        public AddressRL(AddressBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AddressBookModel> GetAllAddress()
        {
            var result = _dbContext.AddressBook.ToList();
            return result;
        }

        public AddressBookModel GetAddressByID(int id)
        {
            var result = _dbContext.AddressBook.Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public AddContactModel AddContact(AddContactModel newContact)
        {
            var result = _dbContext.AddressBook.FirstOrDefault<AddressBookModel>(x => x.Email == newContact.email);

            if (result == null)
            {
                AddressBookModel addressEntry = new AddressBookModel()
                {

                    Name = newContact.name,
                    Address = newContact.address,
                    PhoneNumber = newContact.phone,
                    Email = newContact.email,

                };

                _dbContext.AddressBook.Add(addressEntry);
                _dbContext.SaveChanges();
                return newContact;
            }
            else
            {
                return null;

            }
        }

        public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact)
        {
            var result = _dbContext.AddressBook.FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                result.Name = updateContact.name;
                result.Address = updateContact.address;
                result.PhoneNumber = updateContact.phone;
                result.Email = updateContact.email;

                _dbContext.SaveChanges();
                return updateContact;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteContact(int id)
        {
            var result = _dbContext.AddressBook.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                _dbContext.AddressBook.Remove(result);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}