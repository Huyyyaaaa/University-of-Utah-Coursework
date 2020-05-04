<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;

class MobileLogoutController extends Controller
{
    public function __invoke()
    {
        user()->token()->revoke();

        return $this->success('Successfully Logged Out!');
    }
}
