﻿namespace WebApiChat.Data.Repositories
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using WebApiChat.Data.Interfaces;
    using WebApiChat.Models.Models;

    #endregion

    public class WebApiChatData : IChatData
    {
        private readonly DbContext context;

        private readonly IDictionary<Type, object> repositories;

        public WebApiChatData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<Message> Messages
        {
            get
            {
                return this.GetRepository<Message>();
            }
        }

        public IRepository<Chat> Chats
        {
            get
            {
                return this.GetRepository<Chat>();
            }
        }

        public IRepository<Contact> Contacts
        {
            get
            {
                return this.GetRepository<Contact>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EfRepository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}