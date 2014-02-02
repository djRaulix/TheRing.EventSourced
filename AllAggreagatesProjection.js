
fromAll()
.when({
    $any: function (s, e) {
        if (e.streamId.slice(0, 1) != '$') {
            emit("$AllAggregatesStream", e.eventType, e.body, e.metadata);
        }
    }
})
