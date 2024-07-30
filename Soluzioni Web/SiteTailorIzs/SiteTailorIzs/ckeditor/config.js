/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {

    config.skin = 'softailor';

    config.toolbar_Basic = [
	{ name: 'document', items: ['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink'] }
    ];

    config.toolbar_Minimal = [
    {
        name: 'document', items: ['Bold', 'Italic', 'Underline', 'TextColor', '-', 'NumberedList', 'BulletedList', '-',
          'PasteText', 'PasteFromWord', '-', 'Link', 'Unlink', '-', 'Source', 'Maximize']
    }
    ];

    config.toolbar_Email = [
    {
        name: 'document', items: [
            'Bold', 'Italic', 'Underline', 'TextColor','FontSize', '-',
            'NumberedList', 'BulletedList', '-',
            'JustifyLeft','JustifyCenter','JustifyRight',
          'PasteText', 'PasteFromWord', '-', 'Link', 'Unlink', '-', 'Source', 'Maximize']
    }
    ];

    config.toolbar_Eventailor = [
    { name: 'document', items: ['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', '-', 'PasteText', 'PasteFromWord'] }
    ];

    //config.removePlugins = 'elementspath,magicline';
    config.format_tags = 'p;h1;h2;h3;pre';
    config.removeDialogTabs = 'image:advanced;link:advanced';
    config.enterMode = CKEDITOR.ENTER_DIV;
    config.fontSize_sizes = 'Normale/1em;Grande/1.3em;Grandissimo/1.8em';
};
