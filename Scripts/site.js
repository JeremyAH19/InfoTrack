function ViewModel() {
    var self = this;

    this.keywords = ko.observable().extend({
        required: { message: 'Please enter a value for Keywords.' }
    });

    this.targetUrl = ko.observable().extend({
        required: {
            message: 'Please enter a value for Target URL.'
        },
        isUrl: {
            message: 'Please enter a valid URL.'
        }
    });

    this.responseText = ko.observable();

    this.search = {
        keywords: ko.observable(),
        targetUrl: ko.observable(),
        results: ko.observableArray([-1]),
        isError: ko.observable(false)
    };
    this.hasErrors = ko.pureComputed(function () {
        return self.errors().length > 0;
    })

    this.reset = function () {
        self.keywords(null);
        self.targetUrl(null);

        self.keywords.isModified(false);
        self.targetUrl.isModified(false);
    };

    this.hasClicked = ko.pureComputed(function () {
        return self.search.isError() || self.search.results()[0] !== -1;
    });

    this.hasResults = ko.pureComputed(function () {
        return self.search.results().length > 0 && self.hasClicked();
    });

    this.getResults = function () {
        if (self.hasErrors()) {
            self.errors.showAllMessages();

            return;
        }

        $('#spinner').removeAttr('hidden');

        $.ajax('/Home/GetResults', {
            data: ko.toJSON({ keywords: this.keywords, targetUrl: this.targetUrl }),
            type: 'post', contentType: 'application/json',
            success: function (data) {
                if (data.Success === 'False') {
                    console.log(data.ResponseText);
                    self.search.isError(true);
                } else {
                    self.search.keywords(data.Keywords);
                    self.search.targetUrl(data.TargetUrl);
                    self.search.results(data.SearchResults);
                    self.search.isError(false);
                }

                self.reset();
                self.responseText(data.ResponseText);
                $('#spinner').attr('hidden', 'hidden');
            },
            error: function (er) {
                self.search.isError(true);
                self.reset();
                console.log(er);
            }
        })
    }

    this.errors = ko.validation.group(this);
}

ko.validation.init({
    decorateInputElement: true,
    errorElementClass: 'is-invalid'
});

ko.validation.rules['isUrl'] = {
    validator: function (val) {
        var regex = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;

        return regex.test(val);
    },
    message: 'The field must equal {0}'
};

ko.validation.registerExtenders();

ko.applyBindings(new ViewModel());