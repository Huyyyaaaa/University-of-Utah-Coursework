<?php

namespace App\Http\Controllers\MobileAuth;

use Laravel\Spark\Contracts\Http\Requests\Auth\RegisterRequest;
use Laravel\Spark\Contracts\Interactions\Auth\Register;
use Laravel\Spark\Events\Auth\UserRegistered;
use Laravel\Spark\Http\Controllers\Controller;
use Laravel\Spark\Spark;

class MobileRegisterController extends Controller
{
    public function register(RegisterRequest $request)
    {
        list($user, $paymentId) = Spark::interact(
            Register::class, [$request]
        );

        event(new UserRegistered($user));

        return [
            'status'  => 'success',
            'message' => 'Registration Successful!',
        ];
    }
}
