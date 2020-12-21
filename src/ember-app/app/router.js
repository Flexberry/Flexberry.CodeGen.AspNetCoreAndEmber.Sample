import EmberRouter from '@ember/routing/router';
import config from './config/environment';

const Router = EmberRouter.extend({
  location: config.locationType
});

Router.map(function () {
  this.route('new-platform-super-simple-contact-list-contact-l');
  this.route('new-platform-super-simple-contact-list-contact-e',
  { path: 'new-platform-super-simple-contact-list-contact-e/:id' });
  this.route('new-platform-super-simple-contact-list-contact-e.new',
  { path: 'new-platform-super-simple-contact-list-contact-e/new' });
});

export default Router;
