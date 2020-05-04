<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Hash;
use Laravel\Spark\Spark;

class PasswordController extends Controller
{
    public function __invoke()
    {
        $attributes = $this->validateRequest();

        if (!Hash::check($attributes['current_password'], user()->password)) {
            return $this->error('The given password does not match our records.');
        }

        user()->forceFill([
            'password' => bcrypt($attributes['password'])
        ])->save();

        return $this->success('User Password Successfully Updated!');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'current_password' => 'required',
            'password'         => 'required|confirmed|min:' . Spark::minimumPasswordLength(),
        ]);

        return $attributes;
    }
}
