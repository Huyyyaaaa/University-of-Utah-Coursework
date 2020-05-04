<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateExtraChargesTable extends Migration
{
    public function up()
    {
        Schema::create('extra_charges', function (Blueprint $table) {
            $table->bigIncrements('id');
            $table->unsignedBigInteger('team_id');
            $table->unsignedBigInteger('project_id')->nullable();
            $table->unsignedBigInteger('bid_id')->nullable();
            $table->string('description');
            $table->decimal('amount');
            $table->timestamps();

            $table->foreign('team_id')->references('id')->on('teams');
            $table->foreign('project_id')->references('id')->on('projects');
            $table->foreign('bid_id')->references('id')->on('bids');
        });
    }

    public function down()
    {
        Schema::dropIfExists('extra_charges');
    }
}
