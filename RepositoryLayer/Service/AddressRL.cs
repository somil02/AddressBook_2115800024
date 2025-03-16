using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Text.Json;

public class AddressRL : IAddressRL
{
    private readonly AddressBookDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public AddressRL(AddressBookDbContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public List<AddressBookModel> GetAllAddress()
    {
        string cacheKey = "all_contacts";
        var cachedData = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<AddressBookModel>>(cachedData);
        }

        var result = _dbContext.AddressBook.ToList();
        if (result != null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
        }
        return result;
    }

    public AddressBookModel GetAddressByID(int id)
    {
        string cacheKey = $"contact_{id}";
        var cachedData = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<AddressBookModel>(cachedData);
        }

        var result = _dbContext.AddressBook.FirstOrDefault(x => x.Id == id);
        if (result != null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
        }
        return result;
    }

    public AddContactModel AddContact(AddContactModel newContact)
    {
        var result = _dbContext.AddressBook.FirstOrDefault(x => x.Email == newContact.email);
        if (result == null)
        {
            var addressEntry = new AddressBookModel()
            {
                Name = newContact.name,
                Address = newContact.address,
                PhoneNumber = newContact.phone,
                Email = newContact.email
            };

            _dbContext.AddressBook.Add(addressEntry);
            _dbContext.SaveChanges();

            _cache.Remove("all_contacts"); // Invalidate cache

            return newContact;
        }
        return null;
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

            _cache.Remove("all_contacts");
            _cache.Remove($"contact_{id}");

            return updateContact;
        }
        return null;
    }

    public bool DeleteContact(int id)
    {
        var result = _dbContext.AddressBook.FirstOrDefault(x => x.Id == id);
        if (result != null)
        {
            _dbContext.AddressBook.Remove(result);
            _dbContext.SaveChanges();

            _cache.Remove("all_contacts");
            _cache.Remove($"contact_{id}");

            return true;
        }
        return false;
    }
}