'use strict';

module App.Directives.InputPreFormatter {

    export var directiveName = 'input';

    var directiveFactory = (): () => ng.IDirective => {
        return (): ng.IDirective => {
            var d: ng.IDirective = {
                restrict: 'E',
                require: '?ngModel',
                link: (scope: ng.IScope, element: JQuery, attr: ng.IAttributes, ctrl: ng.INgModelController): any => {

                    var inputType = angular.lowercase(attr['type']);

                    if (!ctrl || inputType === 'radio' || inputType === 'checkbox')
                        return;

                    ctrl.$formatters.unshift((value: string): string => {
                        if (ctrl.$invalid && angular.isUndefined(value) && typeof ctrl.$modelValue === 'string') {
                            return ctrl.$modelValue;
                        } else {
                            return value;
                        }
                    });
                }
            };
            return d;
        };
    };

    export var directive = directiveFactory();
}
