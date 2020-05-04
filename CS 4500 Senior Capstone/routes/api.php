<?php

use Illuminate\Support\Facades\Route;

Route::group([
    'middleware' => 'auth:api'
], function () {
    Route::name('api.')->namespace('Api')->group(function () {
        Route::get('settings/contact', 'ContactInformationController@index')->name('contact-information.index');
        Route::put('settings/contact', 'ContactInformationController@update')->name('contact-information.update');
        Route::put('settings/password', 'PasswordController')->name('password.update');
        Route::resource('material', 'MaterialController', ['only' => ['index', 'store', 'update', 'destroy']]);
        Route::post('material/import', 'MaterialImportController')->name('material.import');
        Route::resource('project-template', 'ProjectTemplateController', ['only' => ['index', 'store', 'update', 'destroy']]);
        Route::resource('project', 'ProjectController', ['only' => ['index', 'store', 'show', 'update', 'destroy']]);
        Route::post('project/{project}/extraCharge/create', 'ExtraChargeController@store')->name('extra-charge.store');
        Route::put('project/{project}/extraCharge/{extraCharge}', 'ExtraChargeController@update')->name('extra-charge.update');
        Route::delete('project/{project}/extraCharge/{extraCharge}', 'ExtraChargeController@destroy')->name('extra-charge.destroy');
        Route::get('project/{project}/photo', 'ProjectPhotoController@index')->name('project.photo.index');
        Route::post('project/{project}/photo', 'ProjectPhotoController@store')->name('project.photo.store');
        Route::delete('project/{project}/photo', 'ProjectPhotoController@destroy')->name('project.photo.delete');
        Route::resource('bid', 'BidController', ['only' => ['index', 'store', 'update', 'destroy']]);
        Route::get('task/{project?}', 'TaskController@index')->name('task.index');
        Route::resource('task', 'TaskController', ['only' => ['store', 'update', 'destroy']]);
        Route::get('task/incomplete/count', 'TaskController@incompleteCount')->name('task.incompleteCount');
        Route::get('dashboard/monthly-team-revenue', 'DashboardController@monthlyTeamRevenue')->name('dashboard.monthly-team-revenue');
        Route::get('dashboard/monthly-individual-revenue', 'DashboardController@monthlyIndividualRevenue')->name('dashboard.monthly-individual-revenue');
        Route::delete('mobile-logout', 'MobileLogoutController')->name('mobile-logout');
    });
});
