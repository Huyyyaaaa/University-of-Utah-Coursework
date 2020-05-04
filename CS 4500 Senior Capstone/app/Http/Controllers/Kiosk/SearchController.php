<?php

namespace App\Http\Controllers\Kiosk;

use Illuminate\Http\Request;
use Laravel\Spark\Contracts\Repositories\UserRepository;
use Laravel\Spark\Http\Controllers\Kiosk\SearchController as SparkSearchController;
use Laravel\Spark\Spark;

class SearchController extends SparkSearchController
{
    /**
     * Get the users based on the incoming search query.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function performBasicSearch(Request $request)
    {
        $query = str_replace('*', '%', $request->input('query'));
        $query = '%' . $query . '%';

        return Spark::interact(UserRepository::class.'@search', [
            $query, $request->user()
        ]);
    }
}
