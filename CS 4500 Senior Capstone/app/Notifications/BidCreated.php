<?php

namespace App\Notifications;

use App\Models\Bid;
use App\Models\User;
use Illuminate\Bus\Queueable;
use Illuminate\Notifications\Notification;
use Laravel\Spark\Notifications\SparkChannel;
use Laravel\Spark\Notifications\SparkNotification;

class BidCreated extends Notification
{
    use Queueable;

    private $user;
    private $bid;

    public function __construct(User $user, Bid $bid)
    {
        $this->user = $user;
        $this->bid  = $bid;
    }

    public function via($notifiable)
    {
        return [SparkChannel::class];
    }

    public function toSpark($notifiable)
    {
        return (new SparkNotification)
            ->action('View Bid', '/bid/' . $this->bid->id)
            ->icon('fa-file-alt')
            ->body($this->user->name . ' created a new bid!');
    }
}
