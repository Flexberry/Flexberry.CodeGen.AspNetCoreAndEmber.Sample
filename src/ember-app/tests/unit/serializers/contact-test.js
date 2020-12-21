import { moduleForModel, test } from 'ember-qunit';

moduleForModel('contact', 'Unit | Serializer | contact', {
  // Specify the other units that are required for this test.
  needs: [
    'serializer:contact',
    'service:syncer',
    'transform:file',
    'transform:decimal',
    'transform:guid',

    'model:contact',
    'validator:ds-error',
    'validator:presence',
    'validator:number',
    'validator:date',
    'validator:belongs-to',
    'validator:has-many',
  ],
});

// Replace this with your real tests.
test('it serializes records', function(assert) {
  let record = this.subject();

  let serializedRecord = record.serialize();

  assert.ok(serializedRecord);
});
