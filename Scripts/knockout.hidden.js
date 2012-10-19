// A ko binding specifically for our CSS, so that we can use CSS classes instead of inline styles in our views.
// (ko's visible binding is incompatible with our hidden class)
ko.bindingHandlers['hidden'] = {
    'update': function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value)
            $(element).addClass('hidden');
        else
            $(element).removeClass('hidden');
    }
};
