﻿#region Copyright
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
// All Rights Reserved
#endregion

using System.Globalization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;
using DotNetNuke.Subscriptions.Components.Common;

namespace DotNetNuke.Subscriptions.Components.Entities
{
    public class SubscriberPropertyAccess : IPropertyAccess
    {
        #region Constructors

        public SubscriberPropertyAccess(Subscriber subscriber)
        {
            _subscriber = subscriber;

            _userInfo = UserController.GetUserById(_subscriber.PortalId, _subscriber.UserId) ??
                        UserController.GetUserById(Null.NullInteger, _subscriber.UserId);

            if (_userInfo == null)
            {
                throw new SubscriptionsException(string.Format("Cannot find user ID {0}", _subscriber.UserId));
            }
        }

        #endregion

        #region Private members

        private readonly Subscriber _subscriber;

        private readonly UserInfo _userInfo;

        #endregion

        #region Implementation of IPropertyAccess

        public string GetProperty(string propertyName, string format, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            switch (propertyName.ToLowerInvariant())
            {
                case "displayname":
                    return _userInfo.DisplayName.ToString(formatProvider);
                case "frequency":
                    return _subscriber.Frequency.ToString(format);
                case "lowerfrequency":
                    return _subscriber.Frequency.ToString(format).ToLower(formatProvider);
                case "objectkey":
                    return _subscriber.ObjectKey.ToString(formatProvider);
                default:
                    propertyNotFound = true;
                    return null;
            }
        }

        public CacheLevel Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }

        #endregion
    }
}