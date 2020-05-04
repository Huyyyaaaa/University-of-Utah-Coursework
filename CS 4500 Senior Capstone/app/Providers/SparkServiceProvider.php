<?php

namespace App\Providers;

use Laravel\Spark\Spark;
use Laravel\Spark\Providers\AppServiceProvider as ServiceProvider;

class SparkServiceProvider extends ServiceProvider
{
    /**
     * Your application and company details.
     *
     * @var array
     */
    protected $details = [
        'vendor' => 'Raise the Roof',
        'product' => 'Contrack',
        'street' => '201 Presidents\' Cir',
        'location' => 'Salt Lake, UT 84112',
        'phone' => '555-555-5555',
    ];

    /**
     * The address where customer support e-mails should be sent.
     *
     * @var string
     */
    protected $sendSupportEmailsTo = null;

    /**
     * All of the application developer e-mail addresses.
     *
     * @var array
     */
    protected $developers = [
        'dev@dev.com'
    ];

    /**
     * Indicates if the application will expose an API.
     *
     * @var bool
     */
    protected $usesApi = false;

    /**
     * Finish configuring Spark for the application.
     *
     * @return void
     */
    public function booted()
    {
        Spark::useStripe()->noCardUpFront()->teamTrialDays(10);

        Spark::teamPlan('Basic Monthly', 'basic-monthly')
            ->price(10)
            ->features([
                'Unlimited Materials',
                'Unlimited Project Templates',
                'Unlimited Bids',
            ]);

        Spark::useRoles([
            'admin' => 'Admin',
            'manager' => 'Manager',
            'employee' => 'Employee',
        ]);
        Spark::useDefaultRole('employee');

        Spark::noAdditionalTeams();

//        Spark::swap(CreateInvitationRequest::class . '@authorize', function () {
//            return $this->user()->isAdminOrAbove();
//        });
    }

    /**
     * Register any application services.
     *
     * @return void
     */
    public function register()
    {
        Spark::useUserModel('App\Models\User');
        Spark::useTeamModel('App\Models\Team');
        parent::register();
    }
}
