import Mixin from '@ember/object/mixin';
import $ from 'jquery';
import DS from 'ember-data';
import { validator } from 'ember-cp-validations';
import { attr, belongsTo, hasMany } from 'ember-flexberry-data/utils/attributes';

export let Model = Mixin.create({
  name: DS.attr('string'),
  value: DS.attr('string')
});

export let ValidationRules = {
  name: {
    descriptionKey: 'models.contact.validations.name.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  value: {
    descriptionKey: 'models.contact.validations.value.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
};

export let defineProjections = function (modelClass) {
  modelClass.defineProjection('ContactE', 'contact', {
    name: attr('Name', { index: 0 }),
    value: attr('Value', { index: 1 })
  });

  modelClass.defineProjection('ContactL', 'contact', {
    name: attr('Name', { index: 0 }),
    value: attr('Value', { index: 1 })
  });
};
