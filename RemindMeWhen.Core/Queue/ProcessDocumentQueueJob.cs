﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knapcode.KitchenSink.Support;
using Knapcode.RemindMeWhen.Core.Clients;
using Knapcode.RemindMeWhen.Core.Logging;
using Knapcode.RemindMeWhen.Core.Models;
using Knapcode.RemindMeWhen.Core.Persistence;
using Knapcode.RemindMeWhen.Core.Support;

namespace Knapcode.RemindMeWhen.Core.Queue
{
    public class ProcessDocumentQueueJob : IQueueJob<ProcessDocumentMessage>
    {
        private readonly IEventSource _eventSource;
        private readonly IDocumentStore _documentStore;
        private readonly IEventExtractor<byte[], MovieReleasedEvent> _deserializer;

        public ProcessDocumentQueueJob(IEventSource eventSource, IDocumentStore documentStore, IEventExtractor<byte[], MovieReleasedEvent> deserializer)
        {
            _eventSource = eventSource;
            _documentStore = documentStore;
            _deserializer = deserializer;
        }

        public async Task ExecuteAsync(ProcessDocumentMessage processDocumentMessage)
        {
            Guard.ArgumentNotNull(processDocumentMessage, "processDocumentMessage");
            if (processDocumentMessage.DocumentMetadata == null)
            {
                throw new ArgumentException("The process document message should not have null metadata.");
            }

            Document document = await _documentStore.GetDocumentAsync(processDocumentMessage.DocumentMetadata.DocumentId, processDocumentMessage.DocumentMetadata.Id);
            if (document == null)
            {
                _eventSource.OnCompletedProcessDocumentJobDueToMissingDocument(processDocumentMessage);
                return;
            }

            IEnumerable<MovieReleasedEvent> movieReleasedEvents = _deserializer.Extract(document.Content);
        }
    }
}