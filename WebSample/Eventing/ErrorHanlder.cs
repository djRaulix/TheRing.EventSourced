﻿namespace WebSample.Eventing
{
    using System;

    using Thering.EventSourced.Eventing;
    using Thering.EventSourced.Eventing.Handlers;

    public class ErrorHanlder : IHandleError
    {
        #region Implementation of IHandleError

        public void HandleError(object @event, Exception exception)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}