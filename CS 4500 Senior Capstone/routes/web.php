<?php

use Illuminate\Support\Facades\Route;

Route::redirect('/home', '/');
Route::get('/landing', 'LandingController');

Route::post('/mobile-login', 'MobileAuth\MobileLoginController@login')->name('mobile-login');
Route::post('/mobile-register', 'MobileAuth\MobileRegisterController@register')->name('mobile-register');

Route::middleware(['auth', 'prevent-back-history'])->group(function () {
    Route::get('/', 'DashboardController@show');

    Route::post('spark/kiosk/users/search', 'Kiosk\SearchController@performBasicSearch');
    Route::put('settings/teams/{team}/information', 'Settings\Teams\TeamInformationController@update');
    Route::post('settings/teams/{team}/invitations', 'Settings\Teams\MailedInvitationController@store');
    Route::delete('settings/invitations/{invitation}', 'Settings\Teams\MailedInvitationController@destroy');

    Route::resource('material', 'MaterialController', ['only' => ['index', 'create', 'show']]);
    Route::resource('project-template', 'ProjectTemplateController', ['only' => ['index', 'create', 'show']]);
    Route::resource('project', 'ProjectController', ['only' => ['index', 'create', 'show']]);
    Route::resource('bid', 'BidController', ['only' => ['show']]);
    Route::get('bid/{bid}/pdf', 'BidController@downloadPdf')->name('bid.pdf');
    Route::get('task', 'TaskController')->name('task.index');
    Route::get('help', 'HelpController');
});

Route::middleware(['auth'])->group(function () {
    //For some reason this route can't be used with the prevent-back-history middleware
    Route::get('material/{material}/export', 'MaterialExportController')->name('material.export');
});
