<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateBidsTable extends Migration
{
    public function up()
    {
        Schema::create('bids', function (Blueprint $table) {
            $table->bigIncrements('id');
            $table->unsignedBigInteger('team_id');
            $table->unsignedBigInteger('project_id');
            $table->string('name');
            $table->text('description');
            $table->string('address');
            $table->string('city');
            $table->string('state');
            $table->string('zip');
            $table->string('contact_name');
            $table->string('contact_phone');
            $table->string('contact_email');
            $table->bigInteger('sq_foot');
            $table->decimal('labor_hours');
            $table->decimal('labor_rate');
            $table->decimal('percentage_on_top');
            $table->decimal('price');
            $table->unsignedBigInteger('created_by_id');
            $table->boolean('won')->default(0);
            $table->timestamps();

            $table->foreign('team_id')->references('id')->on('teams');
            $table->foreign('project_id')->references('id')->on('projects');
            $table->foreign('created_by_id')->references('id')->on('users');
        });
    }

    public function down()
    {
        Schema::dropIfExists('bids');
    }
}
