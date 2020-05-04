<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Bid;
use Illuminate\Support\Carbon;
use Illuminate\Support\Facades\DB;

class DashboardController extends Controller
{
    /** @var Carbon */
    private $from;
    private $to;

    public function __construct()
    {
        $this->from = carbon(request('from'))->startOfDay();
        $this->to   = carbon(request('to'))->endOfDay();
    }

    public function monthlyTeamRevenue()
    {
        $query = Bid::query()->from("bids as b");

        $query
            ->select([
                DB::raw("
                    month(b.created_at) as month,
                    year(b.created_at) as year,
                    concat(concat(date_format(b.created_at, '%b'), ' '), year(b.created_at)) as date,
                    sum(b.price) as total
                ")
            ])
            ->where("b.won", "=", 1)
            ->where("b.team_id", "=", team()->id)
            ->whereBetween("b.created_at", [$this->from, $this->to])
            ->groupBy(
                DB::Raw("month(b.created_at)"),
                DB::Raw("year(b.created_at)"),
                DB::Raw("concat(concat(date_format(b.created_at, '%b'), ' '), year(b.created_at))")
            )
            ->orderBy(DB::raw("year(b.created_at)"))
            ->orderBy(DB::raw("month(b.created_at)"));

        return $query->get();
    }

    public function monthlyIndividualRevenue()
    {
        $query = Bid::query()->from("bids as b")->join("users as u", "b.created_by_id", "=", "u.id");

        $query
            ->select([
                DB::raw("
                    u.name as user,
                    sum(b.price) as total
                ")
            ])
            ->where("b.won", "=", 1)
            ->where("b.team_id", "=", team()->id)
            ->whereBetween("b.created_at", [$this->from, $this->to])
            ->groupBy(
                DB::Raw("u.name")
            );

        return $query->get();
    }
}
