 $.validator.addMethod("regex", function (value, element, params) {
	var match;
	if (this.optional(element)) {
		return true;
	}

	match = new RegExp(params).exec(value);
	return (match && (match.index === 0) && (match[0].length === value.length));
});